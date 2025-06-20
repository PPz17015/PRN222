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

        //GET: Show login page
        public IActionResult Login()
        {
            return View();
        }

       
       //POST: Handle login form submission      
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Please enter both email and password";
                return View();
            }

            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (email.Trim() == adminEmail && password == adminPassword)
            {
                HttpContext.Session.SetString("UserEmail", email);
                HttpContext.Session.SetString("UserRole", "3");
                return RedirectToAction("Dashboard", "Admin");
            }

            var account = _accountService.Login(email, password);
            if (account != null)
            {
                HttpContext.Session.SetString("UserEmail", account.AccountEmail);
                HttpContext.Session.SetString("UserRole", account.AccountRole.ToString());

                
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Invalid email or password";
            return View();
        }

        //GET: Handle user logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
} 