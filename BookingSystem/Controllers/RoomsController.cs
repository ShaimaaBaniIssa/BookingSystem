 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Models;
using Microsoft.Extensions.Hosting;
using BookingSystem.Utility;

namespace BookingSystem.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;


        public RoomsController(ModelContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(decimal? hotelId)
        {
            ViewBag.HotelId = hotelId;
            var modelContext = _context.Rooms.Where(u=>u.Hotelid==hotelId);
            return View(await modelContext.ToListAsync());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Roomid == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create(decimal hotelId)
        {
            ViewBag.hotelId = hotelId;
            ViewData["RoomsType"] = new SelectList(new List<string>
            { SD.RoomType_Deluxe,
            SD.RoomType_Premium,
            SD.RoomType_Luxury,
            SD.RoomType_Double,
            SD.RoomType_Family,
            SD.RoomType_Single}
            );
            return View(new Room() { Hotelid=hotelId});
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Roomid,Name,Roomtype,Description,Price,Maxcapacity,Availabilty,Hotelid,ImageFile,Hotelid")] Room room)
        {
            if (ModelState.IsValid)
            {
                if (room.ImageFile != null)
                {
                    // get rootpath
                    string wwwrootPath = _environment.WebRootPath;

                    // file name
                    string fileName = Guid.NewGuid().ToString() + "_" + room.ImageFile.FileName;

                    // create path
                    // ~/Images/.....
                    string path = Path.Combine(wwwrootPath + "/Images/Room/", fileName);

                    // add the image to Images folder
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await room.ImageFile.CopyToAsync(fileStream);
                    }
                    room.Imagepath = fileName;

                }
                
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { hotelId = room.Hotelid });
            }
            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelid", room.Hotelid);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelid", room.Hotelid);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Roomid,Name,Roomtype,Description,Price,Maxcapacity,Availabilty,Hotelid,Imagepath,ImageFile")] Room room)
        {
            if (id != room.Roomid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // upload new image
                if (room.ImageFile != null)
                {
                    string wwwrootPath = _environment.WebRootPath;

                    if (room.Imagepath != null)
                    {
                        string oldImage = Path.Combine(wwwrootPath + "/Images/Room/", room.Imagepath);

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    // file name
                    string fileName = Guid.NewGuid().ToString() + "_" + room.ImageFile.FileName;

                    // create path
                    // ~/Images/.....
                    string path = Path.Combine(wwwrootPath + "/Images/Room/", fileName);

                    // add the image to Images folder
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await room.ImageFile.CopyToAsync(fileStream);
                    }
                    room.Imagepath = fileName;

                }
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Roomid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new {hotelId=room.Hotelid});
            }
            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelid", room.Hotelid);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(m => m.Roomid == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'ModelContext.Rooms'  is null.");
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { hotelId = room.Hotelid });
        }

        private bool RoomExists(decimal id)
        {
          return (_context.Rooms?.Any(e => e.Roomid == id)).GetValueOrDefault();
        }
    }
}
