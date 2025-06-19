using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject;
using Service;

namespace LeDuyHieuMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Check access rights - only Admin (role 3) and Staff (role 1) can access
        /// </summary>
        private bool CheckAccess()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            return userRole == "3" || userRole == "1";
        }

        /// <summary>
        /// GET: List all categories
        /// </summary>
        public IActionResult Index()
        {
            if (!CheckAccess())
            {
                TempData["ErrorMessage"] = "You do not have permission to access this page.";
                return RedirectToAction("Index", "Home");
            }

            var categories = _categoryService.GetAll();
            return View(categories);
        }

        /// <summary>
        /// GET: Show category details
        /// </summary>
        public IActionResult Details(int? id)
        {
            if (!CheckAccess())
            {
                TempData["ErrorMessage"] = "You do not have permission to access this page.";
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var category = _categoryService.GetById(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        /// <summary>
        /// GET: Show create category form
        /// </summary>
        public IActionResult Create()
        {
            if (!CheckAccess())
            {
                TempData["ErrorMessage"] = "You do not have permission to access this page.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ParentCategories = _categoryService.GetParentCategories();
            return View();
        }

        /// <summary>
        /// POST: Handle create category form submission
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CategoryName,CategoryDescription,ParentCategoryId,Status")] Category category)
        {
            if (!CheckAccess())
            {
                TempData["ErrorMessage"] = "You do not have permission to access this page.";
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                if (_categoryService.Add(category))
                {
                    TempData["SuccessMessage"] = "Category created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMessage = "Cannot create category. Name may already exist or information is invalid.";
                }
            }
            
            ViewBag.ParentCategories = _categoryService.GetParentCategories();
            return View(category);
        }

        /// <summary>
        /// GET: Show edit category form
        /// </summary>
        public IActionResult Edit(int? id)
        {
            if (!CheckAccess())
            {
                TempData["ErrorMessage"] = "You do not have permission to access this page.";
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var category = _categoryService.GetById(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            
            ViewBag.ParentCategories = _categoryService.GetParentCategories();
            return View(category);
        }

        /// <summary>
        /// POST: Handle edit category form submission
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CategoryId,CategoryName,CategoryDescription,ParentCategoryId,Status")] Category category)
        {
            if (!CheckAccess())
            {
                TempData["ErrorMessage"] = "You do not have permission to access this page.";
                return RedirectToAction("Index", "Home");
            }

            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_categoryService.Update(category))
                {
                    TempData["SuccessMessage"] = "Category updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMessage = "Cannot update category. Information is invalid or name already exists.";
                }
            }
            
            ViewBag.ParentCategories = _categoryService.GetParentCategories();
            return View(category);
        }

        /// <summary>
        /// GET: Show delete category confirmation
        /// </summary>
        public IActionResult Delete(int? id)
        {
            if (!CheckAccess())
            {
                TempData["ErrorMessage"] = "You do not have permission to access this page.";
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var category = _categoryService.GetById(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            ViewBag.CanDelete = _categoryService.CanDeleteCategory(id.Value);
            ViewBag.HasChildren = _categoryService.GetChildCategories(id.Value).Any();
            ViewBag.HasArticles = _categoryService.HasNewsArticles(id.Value);
            
            return View(category);
        }

        /// <summary>
        /// POST: Handle category deletion
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!CheckAccess())
            {
                TempData["ErrorMessage"] = "You do not have permission to access this page.";
                return RedirectToAction("Index", "Home");
            }

            if (!_categoryService.CanDeleteCategory(id))
            {
                var hasChildren = _categoryService.GetChildCategories(id).Any();
                var hasArticles = _categoryService.HasNewsArticles(id);

                if (hasChildren && hasArticles)
                {
                    TempData["ErrorMessage"] = "Cannot delete this category because it has subcategories and related articles.";
                }
                else if (hasChildren)
                {
                    TempData["ErrorMessage"] = "Cannot delete this category because it has subcategories.";
                }
                else if (hasArticles)
                {
                    TempData["ErrorMessage"] = "Cannot delete this category because it has related articles.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Cannot delete this category.";
                }
            }
            else if (_categoryService.Delete(id))
            {
                TempData["SuccessMessage"] = "Category deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Cannot delete category.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// API: Get child categories for AJAX requests
        /// </summary>
        public IActionResult GetChildCategories(int parentId)
        {
            if (!CheckAccess())
            {
                return Json(new { error = "Access denied" });
            }

            var childCategories = _categoryService.GetChildCategories(parentId);
            return Json(childCategories.Select(c => new { value = c.CategoryId, text = c.CategoryName }));
        }
    }
}
