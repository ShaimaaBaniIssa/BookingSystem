using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Models;
using BookingSystem.Utility;

namespace BookingSystem.Controllers
{
    
    public class BookingsController : Controller
    {
        private readonly ModelContext _context;

        public BookingsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Search()
        {
            var modelContext = _context.Bookings.Include(b => b.Customer).Include(b => b.Room);
            return View(await modelContext.ToListAsync());
        }
        public async Task<IActionResult> BenefitsReport()
        {
            
            var modelContext = _context.Bookings.Include(b => b.Customer).Include(b => b.Room);
            ViewBag.Benefit = modelContext.Sum(u => u.Totalprice);

            // to get the charts data
            Charts();
            return View(await modelContext.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> BenefitsReport(DateTime? startDate, DateTime? endDate,string? month,string? year)
        {
            var modelContext = _context.Bookings.Include(b => b.Customer).Include(b => b.Room);

            // to get the charts data
            Charts();

            if (startDate != null && endDate == null)
            {
                var result = await modelContext.Where(x => x.BookDate.Value.Date >= startDate).ToListAsync();
                ViewBag.Benefit = result.Sum(u => u.Totalprice);
                return View(result);
            }
            else if (startDate == null && endDate != null)
            {
                var result = await modelContext.Where(x => x.BookDate.Value.Date <= endDate).ToListAsync();
                ViewBag.Benefit = result.Sum(u => u.Totalprice);

                return View(result);
            }
            else if(startDate != null && endDate != null)
            {
                var result = await modelContext
                    .Where(x => x.BookDate.Value.Date <= endDate && x.BookDate.Value.Date >= startDate)
                    .ToListAsync();
                ViewBag.Benefit = result.Sum(u => u.Totalprice);

                return View(result);
            }
            else if (month != null)
            {
                var result = await modelContext
                   .Where(x =>  x.BookDate.Value.Month == Convert.ToInt32(month))
                   .ToListAsync();
                ViewBag.Benefit = result.Sum(u => u.Totalprice);

                return View(result);
            }
            else if (year != null)
            {
                var result = await modelContext
                   .Where(x => x.BookDate.Value.Year == Convert.ToInt32(year))
                   .ToListAsync();
                ViewBag.Benefit = result.Sum(u => u.Totalprice);

                return View(result);
            }
            return View(modelContext);

        }
        public void Charts()
        {
            var booking = _context.Bookings;
            // months chart
            var months = booking
                .OrderBy(u => u.BookDate.Value.Month)
                .GroupBy(u => u.BookDate.Value.Month);
                
            // years chart
            var years = booking
                .OrderBy(u => u.BookDate.Value.Year)
                .GroupBy(u => u.BookDate.Value.Year);

                
            ViewBag.Months = months.Select(grp => new
            {
                month = grp.First().BookDate.Value.Month.ToString(),
                count = grp.Count()
            }).ToArray();

            ViewBag.Years = years.Select(grp => new
            {
                year = grp.First().BookDate.Value.Year.ToString(),
                count = grp.Count()
            }).ToArray();

            ViewBag.MonthsBenefit = months.Select(grp => new
            {
                month = grp.First().BookDate.Value.Month.ToString(),
                count = grp.Sum(u => u.Totalprice)

            });
            ViewBag.YearsBenefit = years.Select(grp => new
            {
                year = grp.First().BookDate.Value.Year.ToString(),
                count = grp.Sum(u => u.Totalprice)
            }).ToArray();

        }
        [HttpPost]
        public async Task<IActionResult> Search(DateTime? startDate, DateTime? endDate)
        {
            var modelContext = _context.Bookings.Include(b => b.Customer).Include(b => b.Room);



            if (startDate == null && endDate == null)
            { return View(modelContext); }

            else if (startDate != null && endDate == null)
            {
                var result = await modelContext.Where(x => x.BookDate.Value.Date >= startDate).ToListAsync();
                return View(result);
            }
            else if (startDate == null && endDate != null)
            {
                var result = await modelContext.Where(x => x.BookDate.Value.Date <= endDate).ToListAsync();
                return View(result);
            }
            else
            {
                var result = await modelContext
                    .Where(x => x.BookDate.Value.Date <= endDate && x.BookDate.Value.Date >= startDate)
                    .ToListAsync();
                return View(result);
            }


        }
        // admin can change the status of the book to check out
        public IActionResult CheckOut(decimal bookingId)
        {
            var booking = _context.Bookings.SingleOrDefault(u => u.Bookingid == bookingId);
            var room = _context.Rooms.SingleOrDefault(u => u.Roomid == booking.Roomid);
            room.BookedFrom = null;
            room.BookedTo = null;

            booking.Status = SD.BookingStatus_CheckedOut;
            _context.Bookings.Update(booking);
            _context.Rooms.Update(room);
            _context.SaveChanges();

            return RedirectToAction(nameof(Search));
        }
        // admin can cancel the book
        public IActionResult CancelBook(decimal bookingId)
        {
            var booking = _context.Bookings.SingleOrDefault(u => u.Bookingid == bookingId);
            var room = _context.Rooms.SingleOrDefault(u => u.Roomid == booking.Roomid);
            room.BookedFrom = null;
            room.BookedTo = null;

            booking.Status = SD.BookingStatus_Cancelled;
            _context.Bookings.Update(booking);
            _context.Rooms.Update(room);
            _context.SaveChanges();

            return RedirectToAction(nameof(Search));
        }
        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.Bookingid == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid");
            ViewData["Roomid"] = new SelectList(_context.Rooms, "Roomid", "Roomid");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Bookingid,Checkin,Checkout,Totalprice,Numberofpersons,Status,Roomid,Customerid")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid", booking.Customerid);
            ViewData["Roomid"] = new SelectList(_context.Rooms, "Roomid", "Roomid", booking.Roomid);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid", booking.Customerid);
            ViewData["Roomid"] = new SelectList(_context.Rooms, "Roomid", "Roomid", booking.Roomid);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Bookingid,Checkin,Checkout,Totalprice,Numberofpersons,Status,Roomid,Customerid")] Booking booking)
        {
            if (id != booking.Bookingid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Bookingid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid", booking.Customerid);
            ViewData["Roomid"] = new SelectList(_context.Rooms, "Roomid", "Roomid", booking.Roomid);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.Bookingid == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'ModelContext.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(decimal id)
        {
          return (_context.Bookings?.Any(e => e.Bookingid == id)).GetValueOrDefault();
        }
    }
}
