using BookingSystem.Models;
using BookingSystem.Services;
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
        private readonly IEmailSender _emailSender;



        public HomeController(ILogger<HomeController> logger, ModelContext context, IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? hotelId, string? pageNumber = "1")
        {

            // view home page data
            var homeData = _context.Homedata.SingleOrDefault(u => u.Title == "Floria - Hotels Booking");

            // add logo path to the seeion to show it on all pages (home layout)
            HttpContext.Session.SetString("LogoPath", homeData.Logopath);
            HttpContext.Session.SetString("DarkLogoPath", homeData.DarkLogopath);

            // to add contact info to the session
            var contactUs = _context.Contactusdata.SingleOrDefault(u => u.HomeId == homeData.HomeId);
            HttpContext.Session.SetString("Email", contactUs.Email);
            HttpContext.Session.SetString("PhoneNumber", contactUs.Phonenumber);
            HttpContext.Session.SetString("Address", contactUs.Address);

            // about us section
            var aboutUs = _context.Aboutusdata.FirstOrDefault(u => u.HomeId == homeData.HomeId);
            ViewBag.AboutUs = aboutUs;

            // for the drop down list
            ViewBag.HotelNames = new SelectList(_context.Hotels, "Hotelid", "Name");
            ViewBag.RoomsType = new SelectList(_context.Rooms.Where(u => u.Hotelid == Convert.ToInt32(hotelId)), "Roomid", "Roomtype");
            if (hotelId != null)
            {
                ViewBag.Hotel = _context.Hotels.Select(u => new
                {
                    Hotelid=u.Hotelid,
                    Name=u.Name,
                }).FirstOrDefault(u=>u.Hotelid== Convert.ToInt32(hotelId));
            }
            // for pagination
            var hotels = _context.Hotels.AsNoTracking().Skip((Convert.ToInt32(pageNumber) - 1) * 3).Take(3).ToList();

            // view all testimonials
            var testimonials = _context.Testimonials.Include(u => u.Customer).Include(u => u.Room).ThenInclude(u => u.Hotel).ToList();

            var tuple = Tuple.Create<IEnumerable<Hotel>, IEnumerable<Testimonial>, Homedatum>(hotels, testimonials, homeData);
            
            return View(tuple);
        }
        [HttpGet]
        public void GetData(string hotelId)
        {
            //List<SelectListItem> data = new List<SelectListItem>();

            //var rooms = _context.Rooms.Where(u => u.Hotelid == Convert.ToInt32(hotelId));
            //foreach (var item in rooms)
            //{
            //    data.Add(new SelectListItem { Value = item.Roomid.ToString(), Text = item.Roomtype });
            //}
            ViewBag.RoomsType = new SelectList(_context.Rooms.Where(u => u.Hotelid == Convert.ToInt32(hotelId)), "Roomid", "Roomtype");


        }
        public async Task<IActionResult> Hotels(string? hotelName, string? pageNumber = "1")
        {
            // if hotel name not null --> seach by hotel name
            if (hotelName != null)
            {
                var result = _context.Hotels.AsNoTracking().ToList()
                    .Where(u => u.Name.Contains(hotelName, StringComparison.OrdinalIgnoreCase)); // ignore case
                return View(result);

            }
            // for pagination
            var hotels = _context.Hotels.AsNoTracking().Skip((Convert.ToInt32(pageNumber) - 1) * 3).Take(3).ToList();

            return View(hotels);
        }

        public IActionResult AboutUs()
        {
            var homeId = _context.Homedata.SingleOrDefault(u => u.Title == "Floria - Hotels Booking").HomeId;

            var aboutUs = _context.Aboutusdata.SingleOrDefault(u => u.HomeId == homeId);
            var rooms = _context.Rooms.Take(4).ToArray();
            ViewBag.Rooms = rooms;
            return View(aboutUs);
        }
        public IActionResult ContactUs()
        {
            var homeId = _context.Homedata.SingleOrDefault(u => u.Title == "Floria - Hotels Booking").HomeId;

            var contactUs = _context.Contactusdata.SingleOrDefault(u => u.HomeId == homeId);
            ViewBag.ContactUs = contactUs;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddReview([Bind("Id,Rtext,Username,Useremail")] Review review)
        {
            // if the user is not logged in --> guest user
            if (HttpContext.Session.GetInt32("Id") == null)
            {
                TempData["warning"] = "Please Login First";
                return RedirectToAction(nameof(ContactUs));

            }
            if (ModelState.IsValid)
            {

                review.Rdate = DateTime.Now;
                _context.Add(review);
                await _context.SaveChangesAsync();
                TempData["success"] = "Your Message sent successfully";
                return RedirectToAction(nameof(ContactUs));
            }
            TempData["warning"] = "Please fill all required data";

            return RedirectToAction(nameof(ContactUs));

        }

        public async Task<IActionResult> Rooms(decimal hotelId)
        {
            // get the rooms for specific hotel
            var modelContext = _context.Rooms.Where(u => u.Hotelid == hotelId).ToList();
            return View(modelContext);
        }
        public IActionResult RoomDetails(decimal roomId)
        {
            string? userName = HttpContext.Session.GetString("Name");
            int? roleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserName = userName;
            ViewBag.RoleId = roleId;

            // retrun the testimonials for this room
            var testimonials = _context.Testimonials.Include(u => u.Customer).Where(u => u.Roomid == roomId
            && u.Status == SD.Testimonial_Approved).ToList();
            ViewBag.Testimonials = testimonials;

            var room = _context.Rooms.SingleOrDefault(u => u.Roomid == roomId);
            return View(room);

        }
       
        public ActionResult AddTestimony(string reviewText, decimal roomId, string rating)
        {
            // check if the user is logged in or no
            if (HttpContext.Session.GetInt32("Id") == null)
            {
                TempData["warning"] = "Please Login First";
                return RedirectToAction(nameof(RoomDetails), new
                {
                    roomId
                });

            }
            Testimonial testimonial = new Testimonial()
            {
                Customerid = HttpContext.Session.GetInt32("Id"),
                Reviewtext = reviewText,
                Roomid = roomId,
                TDate = DateTime.Now,
                Status = SD.Testimonial_Pending,
                Rating = Convert.ToInt32(rating)
            };
            _context.Testimonials.Add(testimonial);
            _context.SaveChanges();
            return RedirectToAction("RoomDetails", new { roomId = roomId });

        }
        public async Task<IActionResult> BookRoom(string? hotelId, string? roomId, DateTime checkIn, DateTime checkOut, string numOfPersons, bool payLater, string source)
        {

            Room? roomData = null;
            var validCheckIn = !checkIn.Year.Equals(0001);
            var validCheckOut = !checkOut.Year.Equals(0001);

            if (roomId != null && hotelId != null && validCheckIn && validCheckOut && numOfPersons != null)
            {
                // check in after check out day
                if (checkIn.CompareTo(checkOut) > 0)
                {
                    TempData["warning"] = "Check in date must be before Check out date";
                    RedirectTo(source, Convert.ToInt32(roomId));
                }
                var hotelData = await _context.Hotels.SingleOrDefaultAsync(u => u.Hotelid == Convert.ToInt32(hotelId));
                // room type is availble in this hotel and less than max Capacity of the room

                roomData = _context.Rooms.SingleOrDefault(
                    r => r.Roomid == Convert.ToInt32(roomId)
            );

                // check all possible cases for room availability
                if (roomData == null)
                {
                    TempData["warning"] = "Room is not found";
                    RedirectTo(source, Convert.ToInt32(roomId));

                }
                if (roomData.Maxcapacity < Convert.ToInt32(numOfPersons))
                {
                    TempData["warning"] = $"Room Max capacity is {roomData.Maxcapacity}";
                    RedirectTo(source, Convert.ToInt32(roomId));

                }
                if (checkIn.Date >= roomData.BookedFrom || checkOut.Date <= roomData.BookedTo)
                {
                    TempData["warning"] = $"Room is unavailable from {roomData.BookedFrom?.ToString("dd MMMM yy")} to {roomData.BookedTo?.ToString("dd MMMM yy")}";
                    RedirectTo(source, Convert.ToInt32(roomId));

                }
                // calculate the days
                int days = (checkOut.Date - checkIn.Date).Days;
                Booking booking = new Booking()
                {
                    Checkin = checkIn,
                    Checkout = checkOut,
                    Customerid = HttpContext.Session.GetInt32("Id"),
                    Roomid = roomData.Roomid,
                    Numberofpersons = Convert.ToInt32(numOfPersons),
                    Totalprice = roomData.Price * days,
                    Status = SD.BookingStatus_Pending,
                    BookDate=DateTime.Now,
                };
                // change the room dates availability
                roomData.BookedFrom = checkIn;
                roomData.BookedTo = checkOut;

                _context.Rooms.Update(roomData);
                _context.Bookings.Add(booking);
                _context.SaveChanges();

                // want to pay later
                if (payLater)
                {
                    return RedirectToAction("UserBookings", "Profile");

                }
                return RedirectToAction("Pay", "Payment",
                                       new
                                       {
                                           bookingId = booking.Bookingid
                                       });

            }

            TempData["warning"] = "Complete the required data";
            return RedirectTo(source, Convert.ToInt32(roomId));

        }
        public IActionResult CancelBook(decimal id)
        {
            //if user want to cancel his book
            var booking = _context.Bookings.SingleOrDefault(u => u.Bookingid == id);
            var room = _context.Rooms.SingleOrDefault(u => u.Roomid == booking.Roomid);

            // reset the dates
            room.BookedFrom = null;
            room.BookedTo = null;

            booking.Status = SD.BookingStatus_Cancelled;

            _context.Bookings.Update(booking);
            _context.Rooms.Update(room);
            _context.SaveChanges();

            return RedirectToAction("UserBookings", "Profile");
        }
        public IActionResult RedirectTo(string name,decimal? roomId)
        {
            if (name.Equals("home"))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(RoomDetails), new {roomId});
            }
        }
    }
}
