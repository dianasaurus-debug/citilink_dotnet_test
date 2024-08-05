using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBooking.DAL;
using RestaurantBooking.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace RestaurantBooking.Controllers
{
    public class MenuController : Controller
    {
        private ReservationDbContext db = new ReservationDbContext();

        [Route("admin/menus")]
        [Authorize]
        public async Task<IActionResult> Index(string? sortOrder, string? searchString, string? currentFilter, int? pageNumber)
        {
            ViewBag.CurrentSort = sortOrder;
            var menus = from menu in db.Menus
                        select menu;
            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                menus = menus.Where(menu => menu.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Title":
                    menus = menus.OrderByDescending(menu => menu.Title);
                    break;
                case "MenuType":
                    menus = menus.OrderByDescending(menu => menu.MenuType);
                    break;
                case "MenuCategory":
                    menus = menus.OrderByDescending(menu => menu.MenuCategoryId);
                    break;
                default:
                    menus = menus.OrderByDescending(menu => menu.ID);
                    break;
            }
            int pageSize = 5;
            var menuList = await PaginatedList<Menu>.CreateAsync(menus.Include(menu => menu.MenuCategory).AsNoTracking(), pageNumber ?? 1, pageSize);

            return View("~/Views/Admin/Menu/Index.cshtml", menuList);
        }
        [Route("admin/menus/detail/{id?}")]
        [Authorize]

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuDetail = await db.Menus
                .Include(menu => menu.MenuCategory)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (menuDetail == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Menu/Detail.cshtml", menuDetail);
        }


        [Route("admin/menus/create")]
        [Authorize]

        public IActionResult Create()
        {
            var menuCategories = db.MenuCategories.ToList();
            ViewBag.menuCategories = menuCategories;
            return View("~/Views/Admin/Menu/Create.cshtml");
        }

        [HttpPost]
        [Route("admin/menus/store")]
        [Authorize]

        public async Task<dynamic> Store([Bind("Title,Description,MenuType,MenuCategoryId,ImageFile")] Menu menu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (menu.ImageFile != null)
                    {
                        var fileName = DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + menu.ImageFile.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await menu.ImageFile.CopyToAsync(stream);
                        }
                        menu.ImagePath = fileName;
                    }
                    db.Menus.Add(menu);
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
            return View("~/Views/Admin/Menu/Create.cshtml");
        }

        [Route("admin/menus/edit/{id?}")]
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuDetail = await db.Menus
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (menuDetail == null)
            {
                return NotFound();
            }
            var menuCategories = db.MenuCategories.ToList();
            ViewBag.menuCategories = menuCategories;
            return View("~/Views/Admin/Menu/Edit.cshtml", menuDetail);
        }

        [HttpPost]
        [Route("admin/menus/update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [Bind("Title,Description,MenuType,MenuCategoryId,ImageFile")] Menu menu)
        {
            
            if (ModelState.IsValid)
            {

                var updatedMenu = db.Menus.FirstOrDefault(x => x.ID == id);
                if (updatedMenu == null)
                {
                    return NotFound();
                }
                if (menu.ImageFile != null)
                {
                    var fileName = DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + menu.ImageFile.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await menu.ImageFile.CopyToAsync(stream);
                    }
                    menu.ImagePath = fileName;
                }

                updatedMenu.Title = menu.Title;
                updatedMenu.Description = menu.Description;
                updatedMenu.MenuType = menu.MenuType;
                updatedMenu.MenuCategoryId = menu.MenuCategoryId;
                updatedMenu.ImagePath = menu.ImagePath;

                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ErrorMessage"] = "Error bcs model is invalid!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("admin/menus/delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var menu = await db.Menus.FindAsync(id);
            if (menu == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                db.Menus.Remove(menu);
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
