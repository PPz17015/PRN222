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

        /// <summary>
        /// GET: List all news articles based on user role
        /// </summary>
        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            List<NewsArticle> articles;

            if (userRole == "2")
            {
                articles = _newsArticleService.GetPublishedArticles();
            }
            else if (userRole == "1")
            {
                articles = _newsArticleService.GetAll();
            }
            else
            {
                articles = _newsArticleService.GetPublishedArticles();
            }

            ViewBag.UserRole = userRole;
            ViewBag.CurrentUserId = GetCurrentUserId();
            return View(articles);
        }

        /// <summary>
        /// GET: Show article details
        /// </summary>
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
            
            if (userRole == "2" && newsArticle.NewsStatus == false)
            {
                return Forbid();
            }

            return View(newsArticle);
        }

        /// <summary>
        /// GET: Show create article form
        /// </summary>
        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            if (userRole != "1")
            {
                return Forbid();
            }

            ViewBag.Categories = _categoryService.GetAll();
            ViewBag.CurrentUserId = GetCurrentUserId();
            return View();
        }

        /// <summary>
        /// POST: Handle create article form submission
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Headline,NewsContent,NewsStatus,CategoryId,AccountId")] NewsArticle newsArticle)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            if (userRole != "1")
            {
                return Forbid();
            }

            var currentUserId = GetCurrentUserId();
            ViewBag.DebugUserId = currentUserId;
            ViewBag.DebugUserEmail = HttpContext.Session.GetString("UserEmail");
            
            if (currentUserId == 0)
            {
                ViewBag.ErrorMessage = "Cannot identify current user. Please log in again.";
                ViewBag.Categories = _categoryService.GetAll();
                return View(newsArticle);
            }

            newsArticle.AccountId = currentUserId;
            newsArticle.CreatedDate = DateTime.Now;

            ModelState.Remove("Account");
            ModelState.Remove("Category");
            ModelState.Remove("AccountId");

            if (newsArticle.CategoryId > 0)
            {
                var category = _categoryService.GetById(newsArticle.CategoryId);
                if (category == null)
                {
                    ModelState.AddModelError("CategoryId", "Selected category does not exist.");
                }
                else
                {
                    ViewBag.DebugCategory = $"Found: {category.CategoryName}";
                }
            }

            if (currentUserId > 0)
            {
                var account = _accountService.GetById(currentUserId);
                ViewBag.DebugAccount = account != null ? $"Found: {account.AccountEmail}" : "Account not found!";
            }

            ViewBag.DebugModelState = ModelState.IsValid;
            ViewBag.DebugData = $"Headline: {newsArticle.Headline}, CategoryId: {newsArticle.CategoryId}, AccountId: {newsArticle.AccountId}";

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
                        TempData["SuccessMessage"] = "Article created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Service.Add() returned false. Check validation or database.";
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

        /// <summary>
        /// GET: Show edit article form
        /// </summary>
        public IActionResult Edit(int? id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            if (userRole != "1")
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

        /// <summary>
        /// POST: Handle edit article form submission
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("NewsArticleId,Headline,NewsContent,NewsStatus,CategoryId,AccountId,CreatedDate,UpdatedBy,UpdatedDate,ModifiedDate")] NewsArticle newsArticle)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            if (userRole != "1")
            {
                return Forbid();
            }

            if (id != newsArticle.NewsArticleId)
            {
                return NotFound();
            }

            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
            {
                ViewBag.ErrorMessage = "Cannot identify current user. Please log in again.";
                ViewBag.Categories = _categoryService.GetAll();
                return View(newsArticle);
            }
            newsArticle.UpdatedBy = currentUserId;
            newsArticle.UpdatedDate = DateTime.Now;
            newsArticle.ModifiedDate = DateTime.Now;

            ModelState.Remove("Account");
            ModelState.Remove("Category");

            // Validate category exists
            if (newsArticle.CategoryId <= 0)
            {
                ModelState.AddModelError("CategoryId", "Please select a valid category.");
            }
            else
            {
                var category = _categoryService.GetById(newsArticle.CategoryId);
                if (category == null)
                {
                    ModelState.AddModelError("CategoryId", "Selected category does not exist.");
                }
            }

            // Validate account exists
            if (newsArticle.AccountId <= 0)
            {
                ModelState.AddModelError("AccountId", "Author information is invalid.");
            }
            else
            {
                var account = _accountService.GetById(newsArticle.AccountId);
                if (account == null)
                {
                    ModelState.AddModelError("AccountId", "Author does not exist.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_newsArticleService.Update(newsArticle))
                    {
                        TempData["SuccessMessage"] = "Article updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Cannot update article. Title may already exist or category is invalid.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Error updating article: {ex.Message}";
                }
            }
            else
            {
                // Debug ModelState errors
                var errors = new List<string>();
                foreach (var modelError in ModelState)
                {
                    foreach (var error in modelError.Value.Errors)
                    {
                        errors.Add($"{modelError.Key}: {error.ErrorMessage}");
                    }
                }
                ViewBag.ErrorMessage = "Validation errors: " + string.Join(" | ", errors);
            }
            
            ViewBag.Categories = _categoryService.GetAll();
            return View(newsArticle);
        }

        /// <summary>
        /// GET: Show delete article confirmation
        /// </summary>
        public IActionResult Delete(int? id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
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

        /// <summary>
        /// POST: Handle article deletion
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            if (userRole != "1")
            {
                return Forbid();
            }

            if (_newsArticleService.Delete(id))
            {
                TempData["SuccessMessage"] = "Article deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Cannot delete article.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// POST: Publish an article
        /// </summary>
        [HttpPost]
        public IActionResult Publish(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            if (userRole != "1" && userRole != "3")
            {
                return Forbid();
            }

            if (_newsArticleService.PublishArticle(id, GetCurrentUserId()))
            {
                TempData["SuccessMessage"] = "Article has been published!";
            }
            else
            {
                TempData["ErrorMessage"] = "Cannot publish article. Article must have content.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// POST: Unpublish an article
        /// </summary>
        [HttpPost]
        public IActionResult Unpublish(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            
            if (userRole != "1" && userRole != "3")
            {
                return Forbid();
            }

            if (_newsArticleService.UnpublishArticle(id, GetCurrentUserId()))
            {
                TempData["SuccessMessage"] = "Article has been unpublished!";
            }
            else
            {
                TempData["ErrorMessage"] = "Cannot unpublish article.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Get current user ID from session
        /// </summary>
        private int GetCurrentUserId()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            
            ViewBag.DebugSessionEmail = userEmail;
            ViewBag.DebugSessionKeys = string.Join(", ", HttpContext.Session.Keys);
            
            if (string.IsNullOrEmpty(userEmail))
            {
                ViewBag.DebugGetUserIdError = "UserEmail is null or empty in session";
                return 0;
            }

            var user = _accountService.GetByEmail(userEmail);
            
            ViewBag.DebugAccountLookup = user != null ? $"Found user: {user.AccountEmail} (ID: {user.AccountId})" : "User not found in database";
            
            return user?.AccountId ?? 0;
        }
    }
}
