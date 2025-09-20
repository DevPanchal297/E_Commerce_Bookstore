using Dev.Models;
using Microsoft.AspNetCore.Mvc;
using Dev.DataAccess.Data;
using Dev.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dev.Models.ViewModels;
using System.Drawing;
using Microsoft.CodeAnalysis.FlowAnalysis;

namespace DevWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnv;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnv)
        {
            _unitOfWork = unitOfWork;
            _webHostEnv = webHostEnv;
        }
        public IActionResult Index()
        {
            List<Product> objCatergoriesList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(objCatergoriesList);
        }

        public IActionResult Upsert(int? id)
        {
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            ProductVM ProductVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                ),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //Create
                return View(ProductVM);
            }
            else
            {
                //Update
                ProductVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(ProductVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM ProductVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnv.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images/product");
                    if (!string.IsNullOrEmpty(ProductVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, ProductVM.Product.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    ProductVM.Product.ImageUrl = @"/images/product/" + fileName;
                }
                if (ProductVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(ProductVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(ProductVM.Product);
                }
                _unitOfWork.Save();
                TempData["Success"] = "Product Created Successfully!";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ProductVM.CategoryList = _unitOfWork.Category.GetAll().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                    );

                return View(ProductVM);
            }
        }
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? Product = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (Product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(Product);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["Success"] = "Product Edited Successfully!";
        //        return RedirectToAction("Index", "Product");
        //    }
        //    return View();
        //}
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? Product = _unitOfWork.Product.Get(u => u.Id == id);
            if (Product == null)
            {
                return NotFound();
            }
            return DeletePOST(id);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? Product = _unitOfWork.Product.Get(u => u.Id == id);
            if (Product == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(Product);
            _unitOfWork.Save();
            TempData["Success"] = "Product Deleted Successfully!";
            //List<Product> cl = _db.Categories.ToList();
            return RedirectToAction("Index", "Product");
        }
    }
}
