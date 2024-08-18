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
using WebApplication1.Models;

namespace BookingSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ModelContext _context;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IWebHostEnvironment _environment;



        public PaymentController(ModelContext context,IPdfGenerator pdfGenerator,IWebHostEnvironment environment)
        {
            _context = context;
            _pdfGenerator = pdfGenerator;
            _environment = environment;
           
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
        public IActionResult Pay(Bankcard bankcard,decimal price,decimal bookingId)
        {
        var card = _context.Bankcards.FirstOrDefault(u=>u.Cardnumber == bankcard.Cardnumber
        //&& u.Cardholdername==bankcard.Cardholdername
        //&& u.CardType == bankcard.CardType
        //&& u.Cvv==bankcard.Cvv
        //&& u.Expirydate==bankcard.Expirydate
       
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
            var booking = _context.Bookings.Include(u=>u.Customer).Include(u=>u.Room)
                .ThenInclude(u=>u.Hotel)
                .FirstOrDefault(u => u.Bookingid == bookingId);
            booking.Status = SD.BookingStatus_Confirmed;
            _context.Bookings.Update(booking);
            _context.SaveChanges();

            Invoice invoice = new Invoice()
            {
                CardNumber = bankcard.Cardnumber, // note substring
                CheckIn = booking.Checkin,
                CheckOut = booking.Checkout,
                CustomerName = $"{booking.Customer.Firstname} {booking.Customer.Lastname}",
                HotelName = booking.Room.Hotel.Name,
                RoomType = booking.Room.Roomtype,
                TotalPrice = booking.Totalprice,
                RoomId = booking.Room.Roomid,
            };

            //var pdf = _pdfGenerator.GetInvoice(invoice).GeneratePdf(); //file name

            //return File(pdf,  "application/pdf", "hello-world.pdf");
            //File(pdf, "application/pdf", "hello-world.pdf");
            string fileName = $"invoice-{invoice.CustomerName.Split(' ')}.pdf";
            string wwwrootPath = _environment.WebRootPath;
            string path = Path.Combine(wwwrootPath + "/Pdf/", fileName);
            _pdfGenerator.GetInvoice(invoice).GeneratePdf(path); //file name

            //string path = Path.Combine(Directory.GetCurrentDirectory()); //,file name

            //.ShowInPreviewer();
            //.GeneratePdfAndShow();
            //.GeneratePdf("invoice.pdf");
            // send 


            // all booking 

            return RedirectToAction("Pay", new { bookingId });

        }
        public ActionResult Invoice()
        {
            return View();
        }

    }
}
