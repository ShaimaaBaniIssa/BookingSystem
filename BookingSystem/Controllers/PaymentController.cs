using BookingSystem.Models;
using BookingSystem.Services;
using BookingSystem.Utility;
using IronPdf.Extensions.Mvc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;


namespace BookingSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ModelContext _context;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailSender _emailSender;



        public PaymentController(ModelContext context,IPdfGenerator pdfGenerator,
            IWebHostEnvironment environment
            ,IEmailSender emailSender)
        {
            _context = context;
            _pdfGenerator = pdfGenerator;
            _environment = environment;
            _emailSender = emailSender;
           
        }
        public IActionResult Pay(decimal bookingId)
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
        [HttpPost]
        public async Task<IActionResult> PayAsync(Bankcard bankcard,decimal price,decimal bookingId)
        {
        var card = _context.Bankcards.FirstOrDefault(u=>u.Cardnumber == bankcard.Cardnumber
        && u.Cardholdername == bankcard.Cardholdername
        && u.CardType == bankcard.CardType
        && u.Cvv == bankcard.Cvv
        );
            if (card == null)
            {
                TempData["error"] = "wrong info";
                return RedirectToAction("Index", new { bookingId });
            }
            var isEqual = card.Expirydate.ToString("dd MMMM yyyy").Equals(bankcard.Expirydate.ToString("dd MMMM yyyy"));
            if (!isEqual)
            {
                TempData["error"] = "wrong info";
                return RedirectToAction("Index", new {bookingId});
            }
            if ( card.Balance < price)
            {
                TempData["error"] = "You dont have enough balance";
                return RedirectToAction("Index", new { bookingId });
            }

            card.Balance = card.Balance - price;
            _context.Bankcards.Update(card);

            var booking = _context.Bookings.Include(u=>u.Customer).Include(u=>u.Room)
                .ThenInclude(u=>u.Hotel)
                .FirstOrDefault(u => u.Bookingid == bookingId);
            booking.Status = SD.BookingStatus_Confirmed;
            _context.Bookings.Update(booking);
            _context.SaveChanges();

            // get logo path
            string wwwrootPath = _environment.WebRootPath;
            string fileName = HttpContext.Session.GetString("LogoPath");
            string path = Path.Combine(wwwrootPath + "/Images/Project/Home/", fileName);

            Invoice invoice = new Invoice()
            {
                CardNumber = $"**** **** **** {bankcard.Cardnumber.Substring(12,4)}",
                CheckIn = booking.Checkin,
                CheckOut = booking.Checkout,
                CustomerName = $"{booking.Customer.Firstname} {booking.Customer.Lastname}",
                HotelName = booking.Room.Hotel.Name,
                RoomType = booking.Room.Roomtype,
                TotalPrice = booking.Totalprice,
                RoomId = booking.Room.Roomid,
                LogoPath = path,
            };

            // to save the pdf file in Pdf folder
            //string fileName = $"invoice_{invoice.CustomerName.Trim()}_{Guid.NewGuid()}.pdf";
            //string wwwrootPath = _environment.WebRootPath;
            //string path = Path.Combine(wwwrootPath + "/Pdf/", fileName);
            //_pdfGenerator.GetInvoice(invoice).GeneratePdf(path);

            // generate the pdf
            var pdf = _pdfGenerator.GetInvoice(invoice).GeneratePdf();
            // send it to the user
            await _emailSender.SendEmail(booking.Customer.Email, "Booking Invoice",
                $"Thank you for choosing {booking.Room.Hotel.Name} for your upcoming stay. We are delighted to have you as our guest and look forward to providing you with an exceptional experience.\n\nPlease find your booking invoice attached to this email for your reference.", 
                pdf);

        
            // all user bookings
            return RedirectToAction("UserBookings","Home");

        }
        public ActionResult Invoice()
        {
            return View();
        }

    }
}
