using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dev.DataAccess.Data;
using Dev.DataAccess.Repository.IRepository;

namespace Dev.DataAccess.Repository
{
    public class Category : Repository<Category> ,ICategoryRepository
    {
        private ApplicationDbContext _db;
        public Category(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Category obj)
        {
            _db.Update(obj);
        }
    }
}
