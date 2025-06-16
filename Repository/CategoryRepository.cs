using BussinessObject;
using DataAccessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<Category> GetAll()
            => new CategoryDAO().GetAll();

        public Category? GetById(int id)
            => new CategoryDAO().GetById(id);

        public List<Category> GetByStatus(bool status)
            => new CategoryDAO().GetByStatus(status);

        public List<Category> GetParentCategories()
            => new CategoryDAO().GetParentCategories();

        public List<Category> GetChildCategories(int parentId)
            => new CategoryDAO().GetChildCategories(parentId);

        public Category? GetByName(string categoryName)
            => new CategoryDAO().GetByName(categoryName);

        public void Add(Category category)
            => new CategoryDAO().Add(category);

        public void Update(Category category)
            => new CategoryDAO().Update(category);

        public void Delete(int id)
            => new CategoryDAO().Delete(id);

        public bool HasChildCategories(int categoryId)
            => new CategoryDAO().HasChildCategories(categoryId);

        public bool HasNewsArticles(int categoryId)
            => new CategoryDAO().HasNewsArticles(categoryId);
    }
}
