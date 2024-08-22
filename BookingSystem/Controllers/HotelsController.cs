using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Models;
using Microsoft.Extensions.Hosting;

namespace BookingSystem.Controllers
{
    public class HotelsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;


        public HotelsController(ModelContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Hotels
        public async Task<IActionResult> Index()
        {
              return _context.Hotels != null ? 
                          View(await _context.Hotels.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Hotels'  is null.");
        }

        // GET: Hotels/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Hotelid == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Hotelid,Name,Address,Email,City,Country,Phonenumber,Description,ImageFile")] Hotel hotel)
        {
            try
            { 
            if (ModelState.IsValid)
            {
                if (hotel.ImageFile != null)
                {
                    // get rootpath
                    string wwwrootPath = _environment.WebRootPath;

                    // file name
                    string fileName = Guid.NewGuid().ToString() + "_" + hotel.ImageFile.FileName;

                    // create path
                    // ~/Images/.....
                    string path = Path.Combine(wwwrootPath + "/Images/Hotel/", fileName);

                    // add the image to Images folder
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await hotel.ImageFile.CopyToAsync(fileStream);
                    }
                    hotel.Imagepath = fileName;

                }
                _context.Hotels.Add(hotel);
                await _context.SaveChangesAsync();
            }
            
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));

        }


        // GET: Hotels/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Hotelid,Name,Address,Email,City,Country,Phonenumber,Description,Imagepath,ImageFile")] Hotel hotel)
        {
            if (id != hotel.Hotelid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // upload new image
                if (hotel.ImageFile != null)
                {
                    string wwwrootPath = _environment.WebRootPath;

                    if (hotel.Imagepath != null)
                    {
                        string oldImage = Path.Combine(wwwrootPath + "/Images/Hotel/", hotel.Imagepath);

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    // file name
                    string fileName = Guid.NewGuid().ToString() + "_" + hotel.ImageFile.FileName;

                    // create path
                    // ~/Images/.....
                    string path = Path.Combine(wwwrootPath + "/Images/Hotel/", fileName);

                    // add the image to Images folder
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await hotel.ImageFile.CopyToAsync(fileStream);
                    }
                    hotel.Imagepath = fileName;

                }
                try
                {
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.Hotelid))
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
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Hotelid == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Hotels == null)
            {
                return Problem("Entity set 'ModelContext.Hotels'  is null.");
            }
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(decimal id)
        {
          return (_context.Hotels?.Any(e => e.Hotelid == id)).GetValueOrDefault();
        }
    }
}
