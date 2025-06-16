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

        // GET: Categories
        public IActionResult Index()
        {
            var categories = _categoryService.GetAll();
            return View(categories);
        }

        // GET: Categories/Details/5
        public IActionResult Details(int? id)
        {
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

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewBag.ParentCategories = _categoryService.GetParentCategories();
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CategoryName,CategoryDescription,ParentCategoryId,Status")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (_categoryService.Add(category))
                {
                    TempData["SuccessMessage"] = "Thêm thể loại thành công!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMessage = "Không thể thêm thể loại. Tên có thể đã tồn tại hoặc thông tin không hợp lệ.";
                }
            }
            
            ViewBag.ParentCategories = _categoryService.GetParentCategories();
            return View(category);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int? id)
        {
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

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CategoryId,CategoryName,CategoryDescription,ParentCategoryId,Status")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_categoryService.Update(category))
                {
                    TempData["SuccessMessage"] = "Cập nhật thể loại thành công!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMessage = "Không thể cập nhật thể loại. Thông tin không hợp lệ hoặc tên đã tồn tại.";
                }
            }
            
            ViewBag.ParentCategories = _categoryService.GetParentCategories();
            return View(category);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _categoryService.GetById(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            // Kiểm tra có thể xóa không
            ViewBag.CanDelete = _categoryService.CanDeleteCategory(id.Value);
            ViewBag.HasChildren = _categoryService.GetChildCategories(id.Value).Any();
            
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_categoryService.CanDeleteCategory(id))
            {
                TempData["ErrorMessage"] = "Không thể xóa thể loại này. Thể loại có thể có thể loại con hoặc bài viết liên quan.";
            }
            else if (_categoryService.Delete(id))
            {
                TempData["SuccessMessage"] = "Xóa thể loại thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể xóa thể loại.";
            }

            return RedirectToAction(nameof(Index));
        }

        // API: Get child categories for AJAX
        public IActionResult GetChildCategories(int parentId)
        {
            var childCategories = _categoryService.GetChildCategories(parentId);
            return Json(childCategories.Select(c => new { value = c.CategoryId, text = c.CategoryName }));
        }
    }
}
