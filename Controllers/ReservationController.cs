using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBooking.DAL;
using RestaurantBooking.Models;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;

namespace RestaurantBooking.Controllers
{
    public class ReservationController : Controller
    {
        private ReservationDbContext db = new ReservationDbContext();

        [Route("/reservation")]
        public IActionResult Index(int? isFailed)
        {
            if (isFailed != null)
            {
                ViewBag.isFailed = isFailed;
            }
            return View("~/Views/Customer/Reservation/Index.cshtml");
        }

        [HttpPost]
        [Route("reservation/store")]
        public async Task<IActionResult> Store(Reservation reservation)
        {
            //try
            //{
            //if (modelstate.isvalid)
            //{

                Customer newCustomer = new Customer { FullName = reservation.Customer.FullName, Phone = reservation.Customer.FullName, Email = reservation.Customer.Email };
                    List<int> usedTables = [];
                    int remainingSeats = reservation.GuestCount;
                    int tableSum = db.GuestTables.Where(g => !db.ReservationTables.Any(gtr => gtr.GuestTableID == g.ID &&
                                                        EntityFunctions.TruncateTime(gtr.Reservation.ReservationTime) == reservation.ReservationTime.Date && reservation.IsValid == true))
                                               .OrderBy(g => Math.Abs(g.SeatCount - remainingSeats))
                                               .Sum(g=>g.SeatCount);
                    if (tableSum < remainingSeats)
                    {
                        return RedirectToAction("Index", new { isFailed = 1 });

                    }
                    while (remainingSeats > 0)
                    {
                        GuestTable availableTable = db.GuestTables
                                                        .Where(g => !usedTables.Contains(g.ID))
                                                        .Where(g => !db.ReservationTables.Any(gtr => gtr.GuestTableID == g.ID &&
                                                        EntityFunctions.TruncateTime(gtr.Reservation.ReservationTime) == reservation.ReservationTime.Date && reservation.IsValid == true))
                                                        .OrderBy(g => Math.Abs(g.SeatCount - remainingSeats))
                                                        .First();
                        if (availableTable != null)
                        {
                            remainingSeats = remainingSeats-availableTable.SeatCount;
                            usedTables.Add(availableTable.ID);
                        }
                    }

                    if (usedTables.Count == 0)
                    {
                        return RedirectToAction("Index", new { isFailed = 1 });
                    }

                    db.Customers.AddOrUpdate(cust => cust.Phone, newCustomer);
                    await db.SaveChangesAsync();

                    var existingCustomer = await db.Customers
                                                       .AsNoTracking()
                                                       .FirstOrDefaultAsync(cust => cust.Phone == newCustomer.Phone);
                    if (existingCustomer == null)
                    {
                        return RedirectToAction("Index", new { isFailed = 1 });

                    }
                    string newReservationCode = "RSVP-" + existingCustomer.ID + "-" + reservation.ReservationTime.ToString("ddMMyyyyhhmmss");
                    Reservation newReservation = new Reservation
                    {
                        ReservationCode = newReservationCode,
                        GuestCount = reservation.GuestCount,
                        CustomerId = existingCustomer.ID,
                        ReservationTime = reservation.ReservationTime,
                        IsValid = true
                    };
                    db.Reservations.Add(newReservation);
                    await db.SaveChangesAsync();
                    foreach(int tableId in usedTables)
                    {
                        ReservationTable newReserveTable = new ReservationTable { GuestTableID = tableId, ReservationID = newReservation.ID };
                        db.ReservationTables.Add(newReserveTable);
                        await db.SaveChangesAsync();
                    }
                    return RedirectToAction("Detail", new { id = newReservation.ID });
               // }
            //}
            //catch (DbUpdateException /* ex */)
            //{
            //    //Log the error (uncomment ex variable name and write a log.
            //    ModelState.AddModelError("", "Unable to save changes. " +
            //        "Try again, and if the problem persists " +
            //        "see your system administrator.");
            //}
            //return RedirectToAction("Index" , new { isFailed = 2 });
        }

        [Route("/reservation/detail/{id?}")]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reservationDetail = await db.Reservations
               .Include(r => r.ReservationTables)
               .AsNoTracking()
               .FirstOrDefaultAsync(r => r.ID == id);

            if (reservationDetail == null)
            {
                return NotFound();
            }
            return View("~/Views/Customer/Reservation/Detail.cshtml", reservationDetail);
        }

        [Route("/reservation/mark-done/{id?}")]
        [Authorize]
        public async Task<IActionResult> AdminMarkAsDone(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reservationDetail = db.Reservations
               .AsNoTracking()
               .FirstOrDefault(r => r.ID == id);

            if (reservationDetail == null)
            {
                return NotFound();
            }
            reservationDetail.IsValid = false;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(AdminIndex));
        }

        [Route("admin/reservation")]
        [Authorize]
        public async Task<IActionResult> AdminIndex(string? sortOrder, string? searchString, string? currentFilter, int? pageNumber)
        {
            ViewBag.CurrentSort = sortOrder;
            var reservations = from reservation in db.Reservations
                        select reservation;
            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                reservations = reservations.Where(reservation => reservation.ReservationCode.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "ReservationCode":
                    reservations = reservations.OrderByDescending(reservation => reservation.ReservationCode);
                    break;
                case "ReservationTime":
                    reservations = reservations.OrderByDescending(reservation => reservation.ReservationTime);
                    break;
                default:
                    reservations = reservations.OrderByDescending(reservation => reservation.ReservationTime);
                    break;
            }
            int pageSize = 5;
            var reservationList = await PaginatedList<Reservation>.CreateAsync(reservations.Include(reservation => reservation.Customer).AsNoTracking(), pageNumber ?? 1, pageSize);

            return View("~/Views/Admin/Reservation/Index.cshtml", reservationList);
        }
        [Route("admin/reservation/detail/{id?}")]
        [Authorize]
        public async Task<IActionResult> AdminDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reservationDetail = await db.Reservations
               .Include(r => r.ReservationTables)
               .AsNoTracking()
               .FirstOrDefaultAsync(r => r.ID == id);

            if (reservationDetail == null)
            {
                return NotFound();
            }
            return View("~/Views/Admin/Reservation/Detail.cshtml", reservationDetail);
        }

    }
}
