using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ProductManagementMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AccountMember model)
        {
            if (string.IsNullOrEmpty(model.MemberId) || string.IsNullOrEmpty(model.MemberPassword))
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View(model);
            }

            try
            {
                var user = _accountService.GetAccountById(model.MemberId);

                if (user != null && user.MemberPassword == model.MemberPassword)
                {
                    HttpContext.Session.SetString("UserId", user.MemberId);
                    HttpContext.Session.SetString("Username", user.FullName);
                    HttpContext.Session.SetString("UserRole", user.MemberRole.ToString());

                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during login. Please try again.");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
