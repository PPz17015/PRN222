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
    public class NewsArticlesController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;

        public NewsArticlesController(INewsArticleService newsArticleService, ICategoryService categoryService, IAccountService accountService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _accountService = accountService;
        }

        // GET: NewsArticles
        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            List<NewsArticle> articles;

            if (userRole == "2") // Lecturer - chỉ xem articles published
            {
                articles = _newsArticleService.GetPublishedArticles();
            }
            else if (userRole == "1" || userRole == "3") // Staff hoặc Admin - xem tất cả
            {
                articles = _newsArticleService.GetAll();
            }
            else
            {
                // Chưa đăng nhập - chỉ xem published
                articles = _newsArticleService.GetPublishedArticles();
            }

            ViewBag.UserRole = userRole;
            return View(articles);
        }

        // GET: NewsArticles/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = _newsArticleService.GetById(id.Value);
            if (newsArticle == null)
            {
                return NotFound();
            }

            var userRole = HttpContext.Session.GetString("UserRole");
            
            // Lecturer chỉ được xem articles published
            if (userRole == "2" && newsArticle.NewsStatus == false)
            {
                return Forbid();
            }

            return View(newsArticle);
        }

        // GET: NewsArticles/Create
        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            // Chỉ Staff và Admin được tạo article
            if (userRole != "1" && userRole != "3")
            {
                return Forbid();
            }

            ViewBag.Categories = _categoryService.GetAll();
            ViewBag.CurrentUserId = GetCurrentUserId();
            return View();
        }

        // POST: NewsArticles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Headline,NewsContent,NewsStatus,CategoryId,AccountId")] NewsArticle newsArticle)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            // Chỉ Staff và Admin được tạo article
            if (userRole != "1" && userRole != "3")
            {
                return Forbid();
            }

            // Lấy AccountId từ session
            var currentUserId = GetCurrentUserId();
            ViewBag.DebugUserId = currentUserId;
            ViewBag.DebugUserEmail = HttpContext.Session.GetString("UserEmail");
            
            if (currentUserId == 0)
            {
                ViewBag.ErrorMessage = "Không thể xác định người dùng hiện tại. Vui lòng đăng nhập lại.";
                ViewBag.Categories = _categoryService.GetAll();
                return View(newsArticle);
            }

            // Set AccountId
            newsArticle.AccountId = currentUserId;

            // Set CreatedDate (required field)  
            newsArticle.CreatedDate = DateTime.Now;

            // Remove navigation property validation (chúng ta chỉ cần foreign keys)
            ModelState.Remove("Account");
            ModelState.Remove("Category");
            
            // Remove AccountId validation vì chúng ta tự set giá trị này
            ModelState.Remove("AccountId");

            // Validate CategoryId exists
            if (newsArticle.CategoryId > 0)
            {
                var category = _categoryService.GetById(newsArticle.CategoryId);
                if (category == null)
                {
                    ModelState.AddModelError("CategoryId", "Danh mục được chọn không tồn tại.");
                }
                else
                {
                    ViewBag.DebugCategory = $"Found: {category.CategoryName}";
                }
            }

            // Debug: Check if account exists
            if (currentUserId > 0)
            {
                var account = _accountService.GetById(currentUserId);
                ViewBag.DebugAccount = account != null ? $"Found: {account.AccountEmail}" : "Account not found!";
            }

            ViewBag.DebugModelState = ModelState.IsValid;
            ViewBag.DebugData = $"Headline: {newsArticle.Headline}, CategoryId: {newsArticle.CategoryId}, AccountId: {newsArticle.AccountId}";

            // Debug: Show ModelState errors
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var modelError in ModelState)
                {
                    foreach (var error in modelError.Value.Errors)
                    {
                        errors.Add($"{modelError.Key}: {error.ErrorMessage}");
                    }
                }
                ViewBag.DebugModelStateErrors = string.Join(" | ", errors);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = _newsArticleService.Add(newsArticle);
                    ViewBag.DebugServiceResult = result;
                    
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Thêm bài viết thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Service.Add() returned false. Kiểm tra validation hoặc database.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Exception: {ex.Message}";
                    ViewBag.DebugException = ex.InnerException?.Message;
                }
            }
            
            ViewBag.Categories = _categoryService.GetAll();
            return View(newsArticle);
        }

        // GET: NewsArticles/Edit/5
        public IActionResult Edit(int? id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            // Chỉ Staff và Admin được sửa article
            if (userRole != "1" && userRole != "3")
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = _newsArticleService.GetById(id.Value);
            if (newsArticle == null)
            {
                return NotFound();
            }
            
            ViewBag.Categories = _categoryService.GetAll();
            ViewBag.CurrentUserId = GetCurrentUserId();
            return View(newsArticle);
        }

        // POST: NewsArticles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("NewsArticleId,Headline,NewsContent,NewsStatus,CategoryId,AccountId,CreatedDate")] NewsArticle newsArticle)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            // Chỉ Staff và Admin được sửa article
            if (userRole != "1" && userRole != "3")
            {
                return Forbid();
            }

            if (id != newsArticle.NewsArticleId)
            {
                return NotFound();
            }

            // Lấy current user để set UpdatedBy
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
            {
                ViewBag.ErrorMessage = "Không thể xác định người dùng hiện tại. Vui lòng đăng nhập lại.";
                ViewBag.Categories = _categoryService.GetAll();
                return View(newsArticle);
            }

            // Set UpdatedBy
            newsArticle.UpdatedBy = currentUserId;

            // Validate CategoryId exists
            if (newsArticle.CategoryId > 0)
            {
                var category = _categoryService.GetById(newsArticle.CategoryId);
                if (category == null)
                {
                    ModelState.AddModelError("CategoryId", "Danh mục được chọn không tồn tại.");
                }
            }

            // Validate AccountId exists
            if (newsArticle.AccountId > 0)
            {
                var account = _accountService.GetById(newsArticle.AccountId);
                if (account == null)
                {
                    ModelState.AddModelError("AccountId", "Tác giả không tồn tại.");
                }
            }

            if (ModelState.IsValid)
            {
                if (_newsArticleService.Update(newsArticle))
                {
                    TempData["SuccessMessage"] = "Cập nhật bài viết thành công!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMessage = "Không thể cập nhật bài viết. Tiêu đề có thể đã tồn tại.";
                }
            }
            
            ViewBag.Categories = _categoryService.GetAll();
            return View(newsArticle);
        }

        // GET: NewsArticles/Delete/5
        public IActionResult Delete(int? id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            // Chỉ Staff và Admin được xóa article
            if (userRole != "1" && userRole != "3")
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = _newsArticleService.GetById(id.Value);
            if (newsArticle == null)
            {
                return NotFound();
            }

            ViewBag.CanDelete = _newsArticleService.CanDeleteArticle(id.Value);
            return View(newsArticle);
        }

        // POST: NewsArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            if (userRole != "1" )
            {
                return Forbid();
            }

            if (_newsArticleService.Delete(id))
            {
                TempData["SuccessMessage"] = "Xóa bài viết thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể xóa bài viết.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Publish/Unpublish actions
        [HttpPost]
        public IActionResult Publish(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            // Chỉ Staff và Admin được publish
            if (userRole != "1" && userRole != "3")
            {
                return Forbid();
            }

            if (_newsArticleService.PublishArticle(id, GetCurrentUserId()))
            {
                TempData["SuccessMessage"] = "Đã xuất bản bài viết!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể xuất bản bài viết. Bài viết phải có nội dung.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Unpublish(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            // Chỉ Staff và Admin được unpublish
            if (userRole != "1" && userRole != "3")
            {
                return Forbid();
            }

            if (_newsArticleService.UnpublishArticle(id, GetCurrentUserId()))
            {
                TempData["SuccessMessage"] = "Đã hủy xuất bản bài viết!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể hủy xuất bản bài viết.";
            }

            return RedirectToAction(nameof(Index));
        }

        private int GetCurrentUserId()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            
            // Debug: Log session info
            ViewBag.DebugSessionEmail = userEmail;
            ViewBag.DebugSessionKeys = string.Join(", ", HttpContext.Session.Keys);
            
            if (string.IsNullOrEmpty(userEmail))
            {
                ViewBag.DebugGetUserIdError = "UserEmail is null or empty in session";
                return 0;
            }

            var user = _accountService.GetByEmail(userEmail);
            
            // Debug: Log account lookup result
            ViewBag.DebugAccountLookup = user != null ? $"Found user: {user.AccountEmail} (ID: {user.AccountId})" : "User not found in database";
            
            return user?.AccountId ?? 0;
        }
    }
}
