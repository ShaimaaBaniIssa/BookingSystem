using BookingSystem.Models;
using BookingSystem.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace BookingSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ModelContext _context;
        public PaymentController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult Index(decimal bookingId)
        {
            var booking = _context.Bookings.Include(u => u.Room).ThenInclude(u => u.Hotel).SingleOrDefault(u => u.Bookingid == bookingId);
           
            ViewBag.Booking = new
            {
                HotelName = booking.Room.Hotel.Name,
                RoomType = booking.Room.Roomtype,
                CheckIn = String.Format("{0:ddd, MMM d, yyyy}", booking.Checkin),
                CheckOut = String.Format("{0:ddd, MMM d, yyyy}", booking.Checkout),
                Total = booking.Totalprice.ToString(),
                BookingId=bookingId

            };
            return View();
        }
        public IActionResult Pay(Bankcard bankcard,decimal price,decimal bookingId) {
        var card = _context.Bankcards.FirstOrDefault(u=>u.Cardnumber == bankcard.Cardnumber
        && u.Cardholdername==bankcard.Cardholdername
        && u.CardType == bankcard.CardType
        && u.Cvv==bankcard.Cvv
        && u.Expirydate==bankcard.Expirydate
       
        );
            if( card == null )
            {
                TempData["error"] = "wrong info";
                return RedirectToAction("Index", new {bookingId});
            }
            if ( card.Balance < price)
            {
                TempData["error"] = "You dont have enough balance";
                return RedirectToAction("Index", new { bookingId });
            }
            var booking = _context.Bookings.FirstOrDefault(u => u.Bookingid == bookingId);
            booking.Status = SD.BookingStatus_Confirmed;
            _context.Bookings.Update(booking);
            _context.SaveChanges();

            // all booking 
            // send invoice
            return RedirectToAction("Index", new { bookingId });

        }

    }
}
