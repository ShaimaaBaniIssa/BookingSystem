using BookingSystem.Models;
using BookingSystem.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            // get the admin name and id , then store it in the session
            string? userName = HttpContext.Session.GetString("Name");
            int? roleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserName = userName;
            ViewBag.RoleId = roleId;

            ViewBag.NumOfRegisteredUsers = _context.UserLogins.Count();
            ViewBag.NumOfHotels = _context.Hotels.Count();
            ViewBag.NewTestimonials = _context.Testimonials.Where(u => u.Status.Equals(SD.Testimonial_Pending)).Count();

            // get the number of available rooms in each hotel
            IEnumerable<HotelRooms> availableRooms = _context.Rooms.Where(u => u.BookedFrom == null && u.BookedTo == null).Include(r => r.Hotel)
                .GroupBy(u => u.Hotelid)
                .Select(grp => new HotelRooms() 
                { HotelName = grp.First().Hotel.Name,
                    NumOfRooms = grp.Count(),
                    Id = grp.First().Hotelid
                }).ToArray();

            // get the number of booked rooms in each hotel
            IEnumerable<HotelRooms> bookedRooms = _context.Rooms.Where(u => u.BookedFrom != null && u.BookedTo != null).Include(r => r.Hotel)
                .GroupBy(u => u.Hotelid)
                .Select(grp => new HotelRooms()
                {
                    HotelName = grp.First().Hotel.Name,
                    NumOfRooms = grp.Count(),
                    Id = grp.First().Hotelid
                }
                ).ToArray();

      

            var tuple = Tuple.Create<IEnumerable<HotelRooms>, IEnumerable<HotelRooms>>(availableRooms, bookedRooms);
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
            var customer = _context.Customers.SingleOrDefault(u => u.Customerid == customerId);
            return View(customer);


        }
        [HttpPost]
        public async Task<IActionResult> EditProfile([Bind("Customerid,Firstname,Lastname,Email,Phonenumber")] Customer customer)
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
