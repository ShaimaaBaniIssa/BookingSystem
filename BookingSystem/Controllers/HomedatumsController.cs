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
    public class HomedatumsController : Controller
    {
        private readonly ModelContext _context;

        public HomedatumsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Homedatums
        public async Task<IActionResult> Index()
        {
              return _context.Homedata != null ? 
                          View(await _context.Homedata.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Homedata'  is null.");
        }

        // GET: Homedatums/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Homedata == null)
            {
                return NotFound();
            }

            var homedatum = await _context.Homedata
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homedatum == null)
            {
                return NotFound();
            }

            return View(homedatum);
        }

        // GET: Homedatums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Homedatums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Logopath,Title,Description,Imgpath1,Imgpath2,Imgpath3")] Homedatum homedatum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homedatum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homedatum);
        }

        // GET: Homedatums/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Homedata == null)
            {
                return NotFound();
            }

            var homedatum = await _context.Homedata.FindAsync(id);
            if (homedatum == null)
            {
                return NotFound();
            }
            return View(homedatum);
        }

        // POST: Homedatums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Logopath,Title,Description,Imgpath1,Imgpath2,Imgpath3")] Homedatum homedatum)
        {
            if (id != homedatum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homedatum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomedatumExists(homedatum.Id))
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
            return View(homedatum);
        }

        // GET: Homedatums/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Homedata == null)
            {
                return NotFound();
            }

            var homedatum = await _context.Homedata
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homedatum == null)
            {
                return NotFound();
            }

            return View(homedatum);
        }

        // POST: Homedatums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Homedata == null)
            {
                return Problem("Entity set 'ModelContext.Homedata'  is null.");
            }
            var homedatum = await _context.Homedata.FindAsync(id);
            if (homedatum != null)
            {
                _context.Homedata.Remove(homedatum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomedatumExists(decimal id)
        {
          return (_context.Homedata?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
