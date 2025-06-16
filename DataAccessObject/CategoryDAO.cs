using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject
{
    public class CategoryDAO
    {
        public List<Category> GetAll()
        {
            using (var context = new FunewsManagementContext())
            {
                return context.Categories.ToList();
            }
        }

        public Category? GetById(int id)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.Categories.FirstOrDefault(c => c.CategoryId == id);
            }
        }

        public List<Category> GetByStatus(bool status)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.Categories.Where(c => c.Status == status).ToList();
            }
        }

        public List<Category> GetParentCategories()
        {
            using (var context = new FunewsManagementContext())
            {
                return context.Categories.Where(c => c.ParentCategoryId == null).ToList();
            }
        }

        public List<Category> GetChildCategories(int parentId)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.Categories.Where(c => c.ParentCategoryId == parentId).ToList();
            }
        }

        public Category? GetByName(string categoryName)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.Categories.FirstOrDefault(c => c.CategoryName == categoryName);
            }
        }

        public void Add(Category category)
        {
            using (var context = new FunewsManagementContext())
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }

        public void Update(Category category)
        {
            using (var context = new FunewsManagementContext())
            {
                context.Categories.Update(category);
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new FunewsManagementContext())
            {
                var category = context.Categories.FirstOrDefault(c => c.CategoryId == id);
                if (category != null)
                {
                    context.Categories.Remove(category);
                    context.SaveChanges();
                }
            }
        }

        public bool HasChildCategories(int categoryId)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.Categories.Any(c => c.ParentCategoryId == categoryId);
            }
        }

        public bool HasNewsArticles(int categoryId)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles.Any(n => n.CategoryId == categoryId);
            }
        }
    }
}
