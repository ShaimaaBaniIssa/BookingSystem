using BookingSystem.Models;
using BookingSystem.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;


        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            string? userName = HttpContext.Session.GetString("Name");
            int? roleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserName = userName;
            ViewBag.RoleId = roleId;

            ViewData["HotelNames"] = new SelectList(_context.Hotels, "Name", "Name");
            ViewData["RoomsType"] = new SelectList(new List<string>
            { SD.RoomType_Deluxe,
            SD.RoomType_Premium,
            SD.RoomType_Luxury,
            SD.RoomType_Double,
            SD.RoomType_Family,
            SD.RoomType_Single}
            );
            var hotels = _context.Hotels.ToList();
            var tuple = Tuple.Create<IEnumerable<Hotel>>(hotels);


            return View(tuple);
        }
        public async Task<IActionResult> Rooms(decimal hotelId) {
            var modelContext = _context.Rooms.Where(u => u.Hotelid == hotelId).ToList();
            //var result = modelContext.DistinctBy(u => u.Roomtype);
            return View(modelContext);
        }
        public IActionResult RoomDetails(decimal roomId)
        {
            string? userName = HttpContext.Session.GetString("Name");
            int? roleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserName = userName;
            ViewBag.RoleId = roleId;

            var testimonials = _context.Testimonials.Include(u=>u.Customer).Where(u=>u.Roomid==roomId 
            && u.Status==SD.Testimonial_Approved).ToList();
            ViewBag.Testimonials = testimonials;

            var room = _context.Rooms.SingleOrDefault(u=>u.Roomid == roomId);
            return View(room);

        }
        public ActionResult AddTestimony(string reviewText,decimal roomId)
        {
            Testimonial testimonial = new Testimonial()
            {
                Customerid = HttpContext.Session.GetInt32("Id"),
                Reviewtext = reviewText,
                Roomid = roomId,
                TDate = DateTime.Now,
                Status = SD.Testimonial_Pending
            };
            _context.Testimonials.Add(testimonial);
            _context.SaveChanges();
            return RedirectToAction("RoomDetails",new{ roomId=roomId});

        }
        public async Task<IActionResult> BookRoom(string? hotel,string? roomType, string? roomId, DateTime checkIn,DateTime checkOut,string numOfPersons)
        {
            Room? roomData = null;
           
            if (roomType != null && hotel != null)
            {

            var hotelData = await _context.Hotels.FirstOrDefaultAsync(u => u.Name == hotel);
            // room type is availble in this hotel and less than max Capacity of the room
            roomData = await _context.Rooms.Include(u => u.Hotel).FirstOrDefaultAsync(r => r.Roomtype == roomType
            && r.Hotelid == hotelData.Hotelid
            && r.Availabilty == 1
            && r.Maxcapacity >= Convert.ToInt32(numOfPersons));
           
            }
            else if(roomId!=null)
            {
                roomData = _context.Rooms.SingleOrDefault(r => r.Roomid == Convert.ToInt32(roomId)
                && r.Availabilty == 1
            && r.Maxcapacity >= Convert.ToInt32(numOfPersons));

            }
            if (roomData == null)
            {
                TempData["error"] = "Room is Unavailable";
                return RedirectToAction("Index");

            }
            int days = (checkOut.Date - checkIn.Date).Days;
            Booking booking = new Booking()
            {
                Checkin = checkIn,
                Checkout = checkOut,
                Customerid = HttpContext.Session.GetInt32("Id"),
                Roomid = roomData.Roomid,
                Numberofpersons = Convert.ToInt32(numOfPersons),
                Totalprice = roomData.Price * days,
                Status = SD.BookingStatus_Pending
            };
            roomData.Availabilty = 0;
            _context.Rooms.Update(roomData);
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            // note return back to books page
            return RedirectToAction("Index", "Payment",
                new {
                bookingId=booking.Bookingid
            });
        }

        
    }
}
