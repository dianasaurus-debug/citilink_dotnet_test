using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RestaurantBooking.DAL;
using RestaurantBooking.Models;
using System.Security.Claims;
using System.Security.Policy;
using System.Web.Helpers;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantBooking.Controllers
{
    public class AdminController : Controller
    {
        private ReservationDbContext db = new ReservationDbContext();

        [Route("admin/login")]
        public IActionResult LoginView()
        {
            return View("~/Views/Admin/Auth/Login.cshtml");
        }

        [HttpPost]
        [Route("admin/login")]
        public IActionResult LoginPost(string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                var userData = db.Admins.Where(a => a.Email.Equals(Email)).FirstOrDefault();
                if (userData != null)
                {
                    if (Crypto.VerifyHashedPassword(userData.Password, Password) == true)
                    {
                        // Assumption based on user identity exists auth db
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userData.Email),
                        // Add additional claims as needed
                    };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            // Set additional authentication properties if needed
                        };

                        HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);
                        return RedirectToAction("Index", "Home");
                    }

                }
            }
            ViewData["ErrorMessage"] = "Pastikan akun yang Anda masukkan benar!";
            return View("~/Views/Admin/Auth/Login.cshtml");
        }

        [HttpPost]
        [Route("admin/logout")]
        [Authorize]
        public IActionResult Logout()
        {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("~/Views/Admin/Auth/Login.cshtml");
        }

    }
}
