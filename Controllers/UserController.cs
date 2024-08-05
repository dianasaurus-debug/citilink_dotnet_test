using Microsoft.AspNetCore.Mvc;
using RestaurantBooking.DAL;
using System.Data.Entity;

namespace RestaurantBooking.Controllers
{
    public class UserController : Controller
    {
        private ReservationDbContext db = new ReservationDbContext();

        [Route("/")]
        public IActionResult Index()
        {
            return View("~/Views/Customer/Front/Index.cshtml");
        }
        [Route("/our-menus")]
        public IActionResult MenuIndex()
        {
            var menuCategories = from menu in db.MenuCategories
                        select menu;
            ViewBag.foodsByCategory = menuCategories
                .Where(menuCategory => menuCategory.MenuType == Models.MenuType.food)
                .Where(menuCategory => menuCategory.Menus.Count > 0)
                .Include(menuCategory => menuCategory.Menus).ToList();

            ViewBag.drinksByCategory = menuCategories
              .Where(menuCategory => menuCategory.MenuType == Models.MenuType.drink)
              .Where(menuCategory => menuCategory.Menus.Count > 0)
              .Include(menuCategory => menuCategory.Menus).ToList();

            return View("~/Views/Customer/Menu/Index.cshtml");
        }
        [Route("/about-us")]
        public IActionResult About()
        {
            return View("~/Views/Customer/Front/About.cshtml");
        }

        
    }
}
