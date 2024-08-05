using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBooking.DAL;
using RestaurantBooking.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace RestaurantBooking.Controllers
{
    public class GuestTableController : Controller
    {
        private ReservationDbContext db = new ReservationDbContext();

        [Route("admin/guest-tables")]
        [Authorize]
        public async Task<IActionResult> Index(string? sortOrder, string? searchString, string? currentFilter, int? pageNumber)
        {
            ViewBag.CurrentSort = sortOrder;
            var guestTables = from guestTable in db.GuestTables
                        select guestTable;
            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;
            int parsedSearch = 0;
            if (searchString!=null&&Int32.TryParse(searchString, out parsedSearch))
            {
                guestTables = guestTables.Where(guestTable => guestTable.TableNumber.Equals(parsedSearch));
            }
            switch (sortOrder)
            {
                case "TableNumber":
                    guestTables = guestTables.OrderByDescending(guestTable => guestTable.TableNumber);
                    break;
                case "SeatCount":
                    guestTables = guestTables.OrderByDescending(guestTable => guestTable.SeatCount);
                    break;
                default:
                    guestTables = guestTables.OrderByDescending(guestTable => guestTable.TableNumber);
                    break;
            }
            int pageSize = 5;
            var guestTableList = await PaginatedList<GuestTable>.CreateAsync(guestTables.AsNoTracking(), pageNumber ?? 1, pageSize);

            return View("~/Views/Admin/GuestTable/Index.cshtml", guestTableList);
        }

        [Route("admin/guest-tables/detail/{id?}")]
        [Authorize]

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tableDetail = await db.GuestTables
                .Include("Reservations.Customer")
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (tableDetail == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/GuestTable/Detail.cshtml", tableDetail);
        }

        [Route("admin/guest-tables/create")]
        [Authorize]

        public IActionResult Create()
        {
            return View("~/Views/Admin/GuestTable/Create.cshtml");
        }

        [HttpPost]
        [Route("admin/guest-tables/store")]
        [Authorize]

        public async Task<IActionResult> Store([Bind("TableNumber,SeatCount")] GuestTable guestTable)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.GuestTables.Add(guestTable);
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
            return View("~/Views/Admin/GuestTable/Create.cshtml");
        }

        [Route("admin/guest-tables/edit/{id?}")]
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

            return View("~/Views/Admin/GuestTable/Edit.cshtml", menuDetail);
        }

        [HttpPost]
        [Route("admin/guest-tables/update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [Bind("TableNumber,SeatCount")] GuestTable table)
        {

            if (ModelState.IsValid)
            {

                var updatedMenu = db.GuestTables.FirstOrDefault(x => x.ID == id);
                if (updatedMenu == null)
                {
                    return NotFound();
                }


                updatedMenu.TableNumber = table.TableNumber;
                updatedMenu.SeatCount = table.SeatCount;


                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ErrorMessage"] = "Error bcs model is invalid!";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [Route("admin/guest-tables/delete/{id}")]
        [Authorize]

        public async Task<IActionResult> Delete(int id)
        {
            var table = await db.GuestTables.FindAsync(id);
            if (table == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                db.GuestTables.Remove(table);
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
