using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Utility;
using BookingSystem.Models;

namespace BookingSystem.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Testimonials
        public async Task<IActionResult> Index()
        {
            ViewBag.NewTestimonials = _context.Testimonials.Where(u => u.Status.Equals(SD.Testimonial_Pending)).Count();

            var modelContext = _context.Testimonials.Include(t => t.Customer).Include(t => t.Room);
            return View(await modelContext.ToListAsync());
        }
        public async Task<IActionResult> ManageStatus(decimal? id, bool isApproved)
        {
            var testimonial = await _context.Testimonials
                .FirstOrDefaultAsync(m => m.Testimonialid == id);

            if (isApproved)
            {
                testimonial!.Status = SD.Testimonial_Approved;
            }
            else
            {
                testimonial!.Status = SD.Testimonial_Rejected;

            }
            _context.Update(testimonial);
            await _context.SaveChangesAsync();
            var modelContext = _context.Testimonials.Include(t => t.Customer).Include(t => t.Room);
            return RedirectToAction(nameof(Index));

        }
       
    }
}
