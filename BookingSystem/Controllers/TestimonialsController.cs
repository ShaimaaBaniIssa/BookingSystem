using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Models;
using BookingSystem.Utility;

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
            var modelContext = _context.Testimonials.Include(t => t.Customer).Include(t => t.Hotel);
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
            var modelContext = _context.Testimonials.Include(t => t.Customer).Include(t => t.Hotel);
            return RedirectToAction(nameof(Index));

        }
        // GET: Testimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Customer)
                .Include(t => t.Hotel)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Testimonials/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Customerid", "Customerid");
            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelid");
            return View();
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Testimonialid,Reviewtext,Hotelid,Customerid")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                testimonial.Status=SD.Testimonial_Pending;
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Customerid", "Customerid", testimonial.Customerid);
            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelid", testimonial.Hotelid);
            return View(testimonial);
        }

        // GET: Testimonials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Customerid", "Customerid", testimonial.Customerid);
            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelid", testimonial.Hotelid);
            return View(testimonial);
        }

        // POST: Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Testimonialid,Reviewtext,Hotelid,Customerid,Status")] Testimonial testimonial)
        {
            if (id != testimonial.Testimonialid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Testimonialid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Customerid", "Customerid", testimonial.Customerid);
            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelid", testimonial.Hotelid);
            return View(testimonial);
        }

        // GET: Testimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Customer)
                .Include(t => t.Hotel)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials'  is null.");
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(decimal id)
        {
          return (_context.Testimonials?.Any(e => e.Testimonialid == id)).GetValueOrDefault();
        }
    }
}
