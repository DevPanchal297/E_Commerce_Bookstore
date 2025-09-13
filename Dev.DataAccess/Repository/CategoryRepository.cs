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
    public class CategoryRepository : Repository<Category> ,ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category obj)
        {
            _db.Update(obj);
        }
    }
}
