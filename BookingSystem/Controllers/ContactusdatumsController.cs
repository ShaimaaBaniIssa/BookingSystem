using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Models;

namespace BookingSystem.Controllers
{
    public class ContactusdatumsController : Controller
    {
        private readonly ModelContext _context;

        public ContactusdatumsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Contactusdatums
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Contactusdata.Include(c => c.Homedatum);
            return View(await modelContext.ToListAsync());
        }

        // GET: Contactusdatums/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Contactusdata == null)
            {
                return NotFound();
            }

            var contactusdatum = await _context.Contactusdata
                .Include(c => c.Homedatum)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactusdatum == null)
            {
                return NotFound();
            }

            return View(contactusdatum);
        }

        // GET: Contactusdatums/Create
        public IActionResult Create()
        {
            ViewData["HomeId"] = new SelectList(_context.Homedata, "HomeId", "HomeId");
            return View();
        }

        // POST: Contactusdatums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,Email,Phonenumber,Description,Locationurl,HomeId")] Contactusdatum contactusdatum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactusdatum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HomeId"] = new SelectList(_context.Homedata, "HomeId", "HomeId", contactusdatum.HomeId);
            return View(contactusdatum);
        }

        // GET: Contactusdatums/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Contactusdata == null)
            {
                return NotFound();
            }

            var contactusdatum = await _context.Contactusdata.FindAsync(id);
            if (contactusdatum == null)
            {
                return NotFound();
            }
            ViewData["HomeId"] = new SelectList(_context.Homedata, "HomeId", "HomeId", contactusdatum.HomeId);
            return View(contactusdatum);
        }

        // POST: Contactusdatums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Address,Email,Phonenumber,Description,Locationurl,HomeId")] Contactusdatum contactusdatum)
        {
            if (id != contactusdatum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactusdatum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactusdatumExists(contactusdatum.Id))
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
            ViewData["HomeId"] = new SelectList(_context.Homedata, "HomeId", "HomeId", contactusdatum.HomeId);
            return View(contactusdatum);
        }

        // GET: Contactusdatums/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Contactusdata == null)
            {
                return NotFound();
            }

            var contactusdatum = await _context.Contactusdata
                .Include(c => c.Homedatum)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactusdatum == null)
            {
                return NotFound();
            }

            return View(contactusdatum);
        }

        // POST: Contactusdatums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Contactusdata == null)
            {
                return Problem("Entity set 'ModelContext.Contactusdata'  is null.");
            }
            var contactusdatum = await _context.Contactusdata.FindAsync(id);
            if (contactusdatum != null)
            {
                _context.Contactusdata.Remove(contactusdatum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactusdatumExists(decimal id)
        {
          return (_context.Contactusdata?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
