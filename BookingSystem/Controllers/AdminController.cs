using BookingSystem.Models;
using BookingSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookingSystem.Controllers
{
    

    public class AdminController : Controller
    {

        private readonly ModelContext _context;
        
        public AdminController(ModelContext context)
        {
            _context = context;
            
        }
        public IActionResult Index()
        {
            ViewBag.NumOfRegisteredUsers = _context.UserLogins.Count();

            List<HotelRooms> availableRooms = _context.Rooms.Where(u=>u.Availabilty==1).Include(r=>r.Hotel)
                .GroupBy(u => u.Hotelid).Select(grp => new HotelRooms() { HotelName =grp.First().Hotel.Name , NumOfRooms = grp.Count()}).ToList();

            List<HotelRooms> bookedRooms = _context.Rooms.Where(u => u.Availabilty == 0).Include(r => r.Hotel)
                .GroupBy(u => u.Hotelid).Select(grp => new HotelRooms() { HotelName = grp.First().Hotel.Name, NumOfRooms = grp.Count() }).ToList();

            var tuple = Tuple.Create<List<HotelRooms>, List<HotelRooms>>(availableRooms, bookedRooms);
            return View(tuple);
        }
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            var customerId = _context.UserLogins.FirstOrDefault(u => u.Id == userId).Customerid;
            var customer = _context.Customers.SingleOrDefault(u => u.Customerid == customerId);
            return View(customer);

        }
        public IActionResult EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            var customerId = _context.UserLogins.FirstOrDefault(u => u.Id == userId).Customerid;
            var customer = _context.Customers.SingleOrDefault(u=>u.Customerid== customerId);
            return View(customer);

      
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile( [Bind("Customerid,Firstname,Lastname,Email,Phonenumber")] Customer customer)
        {
            
                try
                {
                 
                    _context.Customers.Update(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Profile));

                }
                catch (DbUpdateConcurrencyException)
                {
                   
                        return NotFound();
                   
                }
            
            

        }
    }
}
