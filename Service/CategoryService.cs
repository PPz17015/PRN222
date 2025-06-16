using BussinessObject;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepository repository = new CategoryRepository();

        public List<Category> GetAll()
            => repository.GetAll();

        public Category? GetById(int id)
        {
            if (id <= 0) return null;
            return repository.GetById(id);
        }

        public List<Category> GetActiveCategories()
            => repository.GetByStatus(true);

        public List<Category> GetParentCategories()
            => repository.GetParentCategories();

        public List<Category> GetChildCategories(int parentId)
        {
            if (parentId <= 0) return new List<Category>();
            return repository.GetChildCategories(parentId);
        }

        public bool Add(Category category)
        {
            if (category == null) return false;
            if (string.IsNullOrWhiteSpace(category.CategoryName)) return false;

            // Kiểm tra tên category đã tồn tại chưa
            if (IsCategoryNameExists(category.CategoryName)) return false;

            // Kiểm tra ParentCategoryId hợp lệ (nếu có)
            if (category.ParentCategoryId.HasValue)
            {
                var parentCategory = repository.GetById(category.ParentCategoryId.Value);
                if (parentCategory == null) return false;
            }

            try
            {
                repository.Add(category);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Category category)
        {
            if (category == null || category.CategoryId <= 0) return false;
            if (string.IsNullOrWhiteSpace(category.CategoryName)) return false;

            // Kiểm tra category tồn tại
            var existingCategory = repository.GetById(category.CategoryId);
            if (existingCategory == null) return false;

            // Kiểm tra tên category trùng (trừ chính nó)
            if (IsCategoryNameExists(category.CategoryName, category.CategoryId)) return false;

            // Kiểm tra ParentCategoryId hợp lệ (nếu có)
            if (category.ParentCategoryId.HasValue)
            {
                // Không thể set parent là chính nó
                if (category.ParentCategoryId.Value == category.CategoryId) return false;

                var parentCategory = repository.GetById(category.ParentCategoryId.Value);
                if (parentCategory == null) return false;

                // Không thể set parent là con của chính nó (tránh circular reference)
                if (IsCircularReference(category.CategoryId, category.ParentCategoryId.Value)) return false;
            }

            try
            {
                repository.Update(category);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            if (id <= 0) return false;

            // Kiểm tra có thể xóa không
            if (!CanDeleteCategory(id)) return false;

            try
            {
                repository.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsCategoryNameExists(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName)) return false;
            return repository.GetByName(categoryName.Trim()) != null;
        }

        public bool IsCategoryNameExists(string categoryName, int excludeId)
        {
            if (string.IsNullOrWhiteSpace(categoryName)) return false;
            var existingCategory = repository.GetByName(categoryName.Trim());
            return existingCategory != null && existingCategory.CategoryId != excludeId;
        }

        public bool CanDeleteCategory(int categoryId)
        {
            if (categoryId <= 0) return false;

            // Không thể xóa nếu có category con
            if (repository.HasChildCategories(categoryId)) return false;

            // Không thể xóa nếu có bài viết
            if (repository.HasNewsArticles(categoryId)) return false;

            return true;
        }

        public void UpdateCategoryStatus(int categoryId)
        {
            if (categoryId <= 0) return;

            var category = repository.GetById(categoryId);
            if (category == null) return;

            // Tính status dựa trên việc có bài viết hay không
            var hasArticles = repository.HasNewsArticles(categoryId);
            
            if (category.Status != hasArticles)
            {
                category.Status = hasArticles;
                repository.Update(category);
            }
        }

        private bool IsCircularReference(int categoryId, int parentId)
        {
            var currentParent = repository.GetById(parentId);
            while (currentParent != null)
            {
                if (currentParent.CategoryId == categoryId) return true;
                
                if (currentParent.ParentCategoryId.HasValue)
                {
                    currentParent = repository.GetById(currentParent.ParentCategoryId.Value);
                }
                else
                {
                    break;
                }
            }
            return false;
        }
    }
}
