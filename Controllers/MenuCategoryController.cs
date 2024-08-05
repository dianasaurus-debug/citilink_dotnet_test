using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBooking.DAL;
using RestaurantBooking.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace RestaurantBooking.Controllers
{
    public class MenuCategoryController : Controller
    {
        private ReservationDbContext db = new ReservationDbContext();

        [Route("admin/menu-categories")]
        [Authorize]

        public async Task<IActionResult> Index(string? sortOrder, string? searchString, string? currentFilter, int? pageNumber)
        {
            ViewBag.CurrentSort = sortOrder;
            var menuCategories = from menuCategory in db.MenuCategories
                        select menuCategory;
            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                menuCategories = menuCategories.Where(menuCategories => menuCategories.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Title":
                    menuCategories = menuCategories.OrderByDescending(menuCategory => menuCategory.Title);
                    break;
                case "MenuType":
                    menuCategories = menuCategories.OrderByDescending(menuCategory => menuCategory.MenuType);
                    break;
                default:
                    menuCategories = menuCategories.OrderBy(menuCategory => menuCategory.Title);
                    break;
            }
            int pageSize = 5;
            var menuCategoryList = await PaginatedList<MenuCategory>.CreateAsync(menuCategories.AsNoTracking(), pageNumber ?? 1, pageSize);

            return View("~/Views/Admin/MenuCategory/Index.cshtml", menuCategoryList);
        }

        [Route("admin/menu-categories/detail/{id?}")]
        [Authorize]

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryDetail = await db.MenuCategories
                .Include(category => category.Menus)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (categoryDetail == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/MenuCategory/Detail.cshtml", categoryDetail);
        }

        [Route("admin/menu-categories/create")]
        [Authorize]

        public IActionResult Create()
        {
            return View("~/Views/Admin/MenuCategory/Create.cshtml");
        }

        [HttpPost]
        [Route("admin/menu-categories/store")]
        [Authorize]

        public async Task<IActionResult> Store([Bind("Title,Description,MenuType")] MenuCategory menuCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.MenuCategories.Add(menuCategory);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View("~/Views/Admin/MenuCategory/Create.cshtml");
        }

        [Route("admin/menu-categories/edit/{id?}")]
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuDetail = await db.MenuCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (menuDetail == null)
            {
                return NotFound();
            }
           
            return View("~/Views/Admin/MenuCategory/Edit.cshtml", menuDetail);
        }

        [HttpPost]
        [Route("admin/menu-categories/update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [Bind("Title,Description,MenuType")] MenuCategory menu)
        {

            if (ModelState.IsValid)
            {

                var updatedMenu = db.MenuCategories.FirstOrDefault(x => x.ID == id);
                if (updatedMenu == null)
                {
                    return NotFound();
                }
               

                updatedMenu.Title = menu.Title;
                updatedMenu.Description = menu.Description;
                updatedMenu.MenuType = menu.MenuType;
               

                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ErrorMessage"] = "Error bcs model is invalid!";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [Route("admin/menu-categories/delete/{id}")]
        [Authorize]

        public async Task<IActionResult> Delete(int id)
        {
            var menuCategory = await db.MenuCategories.FindAsync(id);
            if (menuCategory == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                db.MenuCategories.Remove(menuCategory);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

    }
}
