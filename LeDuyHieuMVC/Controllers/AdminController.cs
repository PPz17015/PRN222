using Microsoft.AspNetCore.Mvc;
using Service;
using BussinessObject;

namespace LeDuyHieuMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;

        public AdminController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: Admin/Dashboard
        public IActionResult Dashboard()
        {
            // Kiểm tra session admin
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // GET: Admin/ManageAccounts
        public IActionResult ManageAccounts()
        {
            // Kiểm tra session admin
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var accounts = _accountService.GetAll();
            return View(accounts);
        }

        // GET: Admin/CreateAccount
        public IActionResult CreateAccount()
        {
            // Kiểm tra session admin
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Admin/CreateAccount
        [HttpPost]
        public IActionResult CreateAccount(SystemAccount account)
        {
            // Kiểm tra session admin
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                if (_accountService.Add(account))
                {
                    TempData["SuccessMessage"] = "Thêm tài khoản thành công!";
                    return RedirectToAction("ManageAccounts");
                }
                else
                {
                    ViewBag.ErrorMessage = "Không thể thêm tài khoản. Email có thể đã tồn tại hoặc thông tin không hợp lệ.";
                }
            }

            return View(account);
        }

        // GET: Admin/EditAccount/5
        public IActionResult EditAccount(int id)
        {
            // Kiểm tra session admin
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
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

        // POST: Admin/EditAccount/5
        [HttpPost]
        public IActionResult EditAccount(SystemAccount account)
        {
            // Kiểm tra session admin
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                if (_accountService.Update(account))
                {
                    TempData["SuccessMessage"] = "Cập nhật tài khoản thành công!";
                    return RedirectToAction("ManageAccounts");
                }
                else
                {
                    ViewBag.ErrorMessage = "Không thể cập nhật tài khoản. Thông tin không hợp lệ.";
                }
            }

            return View(account);
        }

        // POST: Admin/DeleteAccount/5
        [HttpPost]
        public IActionResult DeleteAccount(int id)
        {
            // Kiểm tra session admin
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (_accountService.Delete(id))
            {
                TempData["SuccessMessage"] = "Xóa tài khoản thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể xóa tài khoản.";
            }

            return RedirectToAction("ManageAccounts");
        }

        // GET: Admin/ManageNews
        public IActionResult ManageNews()
        {
            // Kiểm tra session admin
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            // TODO: Implement news management
            ViewBag.Message = "Trang quản lý bài đăng - Chưa được triển khai";
            return View();
        }
    }
} 