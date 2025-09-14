using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dev.DataAccess.Data;
using Dev.DataAccess.Repository.IRepository;
using Dev.Models;

namespace Dev.DataAccess.Repository
{
    public class ProductRepository : Repository<Product> , IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            _db.Update(obj);
        }
    }
}
