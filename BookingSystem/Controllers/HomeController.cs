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

        public IActionResult Index(string? hotelId,string? pageNumber = "1")
        {
         

            ViewData["HotelNames"] = new SelectList(_context.Hotels, "Hotelid", "Name");
            //ViewData["RoomsType"] = new SelectList(new List<string>
            //{ SD.RoomType_Deluxe,
            //SD.RoomType_Premium,
            //SD.RoomType_Luxury,
            //SD.RoomType_Double,
            //SD.RoomType_Family,
            //SD.RoomType_Single}
            //);
            
            ViewData["RoomsType"] = new SelectList(_context.Rooms.Where(u => u.Hotelid == Convert.ToInt32(hotelId)), "Roomid", "Roomtype");

            var hotels = _context.Hotels.AsNoTracking().Skip((Convert.ToInt32(pageNumber)-1)*3).Take(3).ToList();
            var testimonials = _context.Testimonials.Include(u => u.Customer).Include(u => u.Room).ThenInclude(u => u.Hotel).ToList();
            var homeData = _context.Homedata.SingleOrDefault(u=>u.Title== "Floria - Hotels Booking");
            HttpContext.Session.SetString( "LogoPath",homeData.Logopath);

            var tuple = Tuple.Create<IEnumerable<Hotel>, IEnumerable<Testimonial>,Homedatum>(hotels, testimonials,homeData);


            return View(tuple);
        }
        
        public async Task<IActionResult> Hotels(string? hotelName ,string? pageNumber = "1")
        {

            if (hotelName != null)
            {
                var result = _context.Hotels.AsNoTracking().ToList()
                    .Where(u => u.Name.Contains(hotelName, StringComparison.OrdinalIgnoreCase));
                  
                return View(result);

            }
            var hotels = _context.Hotels.AsNoTracking().Skip((Convert.ToInt32(pageNumber) - 1) * 3).Take(3).ToList();

            return View(hotels);
        }
       
        public async Task<IActionResult> Rooms(decimal hotelId)
        {
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

            var testimonials = _context.Testimonials.Include(u => u.Customer).Where(u => u.Roomid == roomId
            && u.Status == SD.Testimonial_Approved).ToList();
            ViewBag.Testimonials = testimonials;

            var room = _context.Rooms.SingleOrDefault(u => u.Roomid == roomId);
            return View(room);

        }
        public async Task <IActionResult> Profile()
        {
            var customerId = HttpContext.Session.GetInt32("Id");
            var customer = await _context.Customers.SingleOrDefaultAsync(u=>u.Customerid== customerId);
            return View(customer);
        }
        public async Task <IActionResult> EditProfile()
        {
            var customerId = HttpContext.Session.GetInt32("Id");
            var customer = await _context.Customers.SingleOrDefaultAsync(u => u.Customerid == customerId);

            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(decimal id, [Bind("Customerid,Firstname,Lastname,Email,Phonenumber")] Customer customer)
        {
            if (id != customer.Customerid)
            {
                return NotFound();
            }

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
            var customerId = HttpContext.Session.GetInt32("Id");
            var bookings = await _context.Bookings.Include(u=>u.Room).ThenInclude(u=>u.Hotel).Where(u=>u.Customerid== customerId).ToListAsync();

            return View(bookings);
        }
        public ActionResult AddTestimony(string reviewText, decimal roomId,string rating)
        {
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
        public async Task<IActionResult> BookRoom(string? hotelId, string? roomId, DateTime checkIn, DateTime checkOut, string numOfPersons, bool pay)
        {
            Room? roomData = null;

            if (roomId != null && hotelId != null && checkIn != null && checkOut != null && numOfPersons != null)
            {

                var hotelData = await _context.Hotels.SingleOrDefaultAsync(u => u.Hotelid == Convert.ToInt32(hotelId));
                // room type is availble in this hotel and less than max Capacity of the room

                roomData = _context.Rooms.SingleOrDefault(
                    r => r.Roomid == Convert.ToInt32(roomId)
            //&& r.Maxcapacity >= Convert.ToInt32(numOfPersons)
            );


                if (roomData == null)
                {
                    TempData["warning"] = "Room is not found";
                    return RedirectToAction("Index");

                }
                if (roomData.Maxcapacity < Convert.ToInt32(numOfPersons))
                {
                    TempData["warning"] = $"Room Max capacity is {roomData.Maxcapacity}";
                    return RedirectToAction("Index");

                }
                if (checkIn.Date >= roomData.BookedFrom || checkOut.Date <= roomData.BookedTo)
                {
                    TempData["warning"] = $"Room is unavailable from {roomData.BookedFrom?.ToString("dd MMMM yy")} to {roomData.BookedTo?.ToString("dd MMMM yy")}";
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

                roomData.BookedFrom = checkIn;
                roomData.BookedTo = checkOut;

                _context.Rooms.Update(roomData);
                _context.Bookings.Add(booking);
                _context.SaveChanges();

                if (pay)
                {
                    return RedirectToAction(nameof(UserBookings));

                }
                return RedirectToAction("Pay", "Payment",
                                       new
                                       {
                                           bookingId = booking.Bookingid
                                       });

            }

            TempData["warning"] = "Complete the required data";
            return RedirectToAction("Index");

        }
        public IActionResult CancelBook(decimal id)
        {
            var booking = _context.Bookings.SingleOrDefault(u=>u.Bookingid == id);
            var room = _context.Rooms.SingleOrDefault(u => u.Roomid == booking.Roomid);
            room.BookedFrom = null;
            room.BookedTo = null;

            booking.Status = SD.BookingStatus_Cancelled;
            _context.Bookings.Update(booking);
            _context.Rooms.Update(room);
            _context.SaveChanges();

            return RedirectToAction(nameof(UserBookings));
        }

    }
}
