using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Category? GetById(int id);
        List<Category> GetActiveCategories();
        List<Category> GetParentCategories();
        List<Category> GetChildCategories(int parentId);
        bool Add(Category category);
        bool Update(Category category);
        bool Delete(int id);
        bool IsCategoryNameExists(string categoryName);
        bool IsCategoryNameExists(string categoryName, int excludeId);
        bool CanDeleteCategory(int categoryId);
        void UpdateCategoryStatus(int categoryId);
    }
}
