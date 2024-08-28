using BookingSystem.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ModelContext _context;

        public ProfileController(ModelContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Profile()
        {
            var customerId = HttpContext.Session.GetInt32("Id");
            var customer = await _context.Customers.SingleOrDefaultAsync(u => u.Customerid == customerId);
            return View(customer);
        }
        public async Task<IActionResult> EditProfile()
        {
            var customerId = HttpContext.Session.GetInt32("Id");
            var customer = await _context.Customers.SingleOrDefaultAsync(u => u.Customerid == customerId);

            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile([Bind("Customerid,Firstname,Lastname,Email,Phonenumber")] Customer customer)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["error"] = "Error";
                }
                return RedirectToAction(nameof(Profile));
            }
            return View(customer);
        }
        public async Task<IActionResult> UserBookings()
        {
            // return all user bookings
            var customerId = HttpContext.Session.GetInt32("Id");
            var bookings = await _context.Bookings.Include(u => u.Room).ThenInclude(u => u.Hotel).Where(u => u.Customerid == customerId).ToListAsync();

            return View(bookings);
        }
    }
}
