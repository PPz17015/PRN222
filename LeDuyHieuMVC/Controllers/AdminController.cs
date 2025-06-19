using Microsoft.AspNetCore.Mvc;
using Service;
using BussinessObject;

namespace LeDuyHieuMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly INewsArticleService _newsArticleService;

        public AdminController(IAccountService accountService, INewsArticleService newsArticleService)
        {
            _accountService = accountService;
            _newsArticleService = newsArticleService;
        }

        /// <summary>
        /// GET: Admin dashboard page
        /// </summary>
        public IActionResult Dashboard()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "3")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        /// <summary>
        /// GET: List all accounts for management
        /// </summary>
        public IActionResult ManageAccounts()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "3")
            {
                return RedirectToAction("Login", "Account");
            }

            var accounts = _accountService.GetAll();
            return View(accounts);
        }

        /// <summary>
        /// GET: Show create account form
        /// </summary>
        public IActionResult CreateAccount()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "3")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        /// <summary>
        /// POST: Handle create account form submission
        /// </summary>
        [HttpPost]
        public IActionResult CreateAccount(SystemAccount account)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "3")
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                if (_accountService.Add(account))
                {
                    TempData["SuccessMessage"] = "Account created successfully!";
                    return RedirectToAction("ManageAccounts");
                }
                else
                {
                    ViewBag.ErrorMessage = "Cannot create account. Email may already exist or information is invalid.";
                }
            }

            return View(account);
        }

        /// <summary>
        /// GET: Show edit account form
        /// </summary>
        public IActionResult EditAccount(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "3")
            {
                return RedirectToAction("Login", "Account");
            }

            var account = _accountService.GetById(id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        /// <summary>
        /// POST: Handle edit account form submission
        /// </summary>
        [HttpPost]
        public IActionResult EditAccount(SystemAccount account)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "3")
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                if (_accountService.Update(account))
                {
                    TempData["SuccessMessage"] = "Account updated successfully!";
                    return RedirectToAction("ManageAccounts");
                }
                else
                {
                    ViewBag.ErrorMessage = "Cannot update account. Information is invalid.";
                }
            }

            return View(account);
        }

        /// <summary>
        /// POST: Handle account deletion
        /// </summary>
        [HttpPost]
        public IActionResult DeleteAccount(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "3")
            {
                return RedirectToAction("Login", "Account");
            }

            if (_accountService.Delete(id))
            {
                TempData["SuccessMessage"] = "Account deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Cannot delete account.";
            }

            return RedirectToAction("ManageAccounts");
        }

        /// <summary>
        /// GET: Show news management page
        /// </summary>
        public IActionResult ManageNews()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "3")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Message = "News Management Page - Not yet implemented";
            return View();
        }

        /// <summary>
        /// GET: Show articles management page with date filter
        /// </summary>
        public IActionResult ManageArticles(DateTime? fromDate, DateTime? toDate)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "3")
            {
                return RedirectToAction("Login", "Account");
            }

            var allArticles = _newsArticleService.GetAll();
            var originalFromDate = fromDate;
            var originalToDate = toDate;
            if (!fromDate.HasValue)
                fromDate = DateTime.Now.AddMonths(-1);
            if (!toDate.HasValue)
                toDate = DateTime.Now;
            var filterFromDate = fromDate.Value.Date;
            var filterToDate = toDate.Value.Date;
            var filteredArticles = new List<NewsArticle>();
            var debugDetails = new List<string>();

            foreach (var article in allArticles)
            {
                var articleDate = article.CreatedDate.Date;
                var isInRange = articleDate >= filterFromDate && articleDate <= filterToDate;
                
                debugDetails.Add($"ID:{article.NewsArticleId} Date:{articleDate:yyyy-MM-dd} InRange:{isInRange}");
                
                if (isInRange)
                {
                    filteredArticles.Add(article);
                }
            }

            filteredArticles = filteredArticles.OrderByDescending(a => a.CreatedDate).ToList();

            ViewBag.FromDate = fromDate.Value.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate.Value.ToString("yyyy-MM-dd");
            ViewBag.TotalArticles = filteredArticles.Count;
            ViewBag.PublishedCount = filteredArticles.Count(a => a.NewsStatus);
            ViewBag.DraftCount = filteredArticles.Count(a => !a.NewsStatus);

            return View(filteredArticles);
        }
    }
} 