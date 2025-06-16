using Microsoft.AspNetCore.Mvc;
using Service;
using BussinessObject;

namespace LeDuyHieuMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ email và mật khẩu";
                return View();
            }

            // Kiểm tra admin account từ appsettings
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (email.Trim() == adminEmail && password == adminPassword)
            {
                // Admin login - chuyển tới trang quản lý Admin
                HttpContext.Session.SetString("UserEmail", email);
                HttpContext.Session.SetString("UserRole", "3"); // Admin role = 3
                return RedirectToAction("Dashboard", "Admin");
            }

            // Kiểm tra tài khoản trong database
            var account = _accountService.Login(email, password);
            if (account != null)
            {
                HttpContext.Session.SetString("UserEmail", account.AccountEmail);
                HttpContext.Session.SetString("UserRole", account.AccountRole.ToString());

                // Staff và Lecturer chuyển về trang chủ
                string roleMessage = account.AccountRole switch
                {
                    1 => "Đăng nhập thành công với quyền Staff!",
                    2 => "Đăng nhập thành công với quyền Lecturer!",
                    _ => "Đăng nhập thành công!"
                };

                TempData["SuccessMessage"] = roleMessage;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Email hoặc mật khẩu không đúng";
            return View();
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["InfoMessage"] = "Bạn đã đăng xuất thành công!";
            return RedirectToAction("Login");
        }
    }
} 