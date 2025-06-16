using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category? GetById(int id);
        List<Category> GetByStatus(bool status);
        List<Category> GetParentCategories();
        List<Category> GetChildCategories(int parentId);
        Category? GetByName(string categoryName);
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);
        bool HasChildCategories(int categoryId);
        bool HasNewsArticles(int categoryId);
    }
}
