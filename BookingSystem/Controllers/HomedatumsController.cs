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
    public class HomedatumsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;


        public HomedatumsController(ModelContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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
        public async Task<IActionResult> Create([Bind("Id,Logo,Title,Description,ImageFile1,ImageFile2,ImageFile3")] Homedatum homedatum)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _environment.WebRootPath;
                if (homedatum.Logo != null)
                {

                    string fileName = Guid.NewGuid().ToString() + "_" + homedatum.Logo.FileName;

                    string path = Path.Combine(wwwrootPath + "/Images/Project/Home/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homedatum.Logo.CopyToAsync(fileStream);
                    }
                    homedatum.Logopath = fileName;

                }
                if (homedatum.ImageFile1 != null)
                {
                    
                    string fileName = Guid.NewGuid().ToString() + "_" + homedatum.ImageFile1.FileName;

                    string path = Path.Combine(wwwrootPath + "/Images/Project/Home/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homedatum.ImageFile1.CopyToAsync(fileStream);
                    }
                    homedatum.Imgpath1 = fileName;

                }
                if (homedatum.ImageFile2 != null)
                {

                    string fileName = Guid.NewGuid().ToString() + "_" + homedatum.ImageFile2.FileName;

                    string path = Path.Combine(wwwrootPath + "/Images/Project/Home/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homedatum.ImageFile2.CopyToAsync(fileStream);
                    }
                    homedatum.Imgpath2 = fileName;

                }
                if (homedatum.ImageFile3 != null)
                {

                    string fileName = Guid.NewGuid().ToString() + "_" + homedatum.ImageFile3.FileName;

                    string path = Path.Combine(wwwrootPath + "/Images/Project/Home/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homedatum.ImageFile3.CopyToAsync(fileStream);
                    }
                    homedatum.Imgpath3 = fileName;

                }
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
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Logopath,Title,Description,Imgpath1,Imgpath2,Imgpath3,ImageFile1,ImageFile2,ImageFile3,Logo")] Homedatum homedatum)
        {
            if (id != homedatum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (homedatum.ImageFile1 != null)
                {
                    string wwwrootPath = _environment.WebRootPath;
                    // delete the old image
                    if (homedatum.Imgpath1 != null)
                    {
                        string oldImage = Path.Combine(wwwrootPath + "/Images/Project/Home/", homedatum.Imgpath1);

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    // file name
                    string fileName = Guid.NewGuid().ToString() + "_" + homedatum.ImageFile1.FileName;

                    // create path
                    // ~/Images/.....
                    string path = Path.Combine(wwwrootPath + "/Images/Project/Home/", fileName);

                    // add the image to Images folder
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homedatum.ImageFile1.CopyToAsync(fileStream);
                    }
                    homedatum.Imgpath1 = fileName;

                }
                if (homedatum.ImageFile2 != null)
                {
                    string wwwrootPath = _environment.WebRootPath;
                    // delete the old image
                    if (homedatum.Imgpath2 != null)
                    {
                        string oldImage = Path.Combine(wwwrootPath + "/Images/Project/Home/", homedatum.Imgpath2);

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    // file name
                    string fileName = Guid.NewGuid().ToString() + "_" + homedatum.ImageFile2.FileName;

                    // create path
                    // ~/Images/.....
                    string path = Path.Combine(wwwrootPath + "/Images/Project/Home/", fileName);

                    // add the image to Images folder
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homedatum.ImageFile2.CopyToAsync(fileStream);
                    }
                    homedatum.Imgpath2 = fileName;

                }
                if (homedatum.ImageFile3 != null)
                {
                    string wwwrootPath = _environment.WebRootPath;
                    // delete the old image
                    if (homedatum.Imgpath3 != null)
                    {
                        string oldImage = Path.Combine(wwwrootPath + "/Images/Project/Home/", homedatum.Imgpath3);

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    // file name
                    string fileName = Guid.NewGuid().ToString() + "_" + homedatum.ImageFile3.FileName;

                    // create path
                    // ~/Images/.....
                    string path = Path.Combine(wwwrootPath + "/Images/Project/Home/", fileName);

                    // add the image to Images folder
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homedatum.ImageFile3.CopyToAsync(fileStream);
                    }
                    homedatum.Imgpath3 = fileName;

                }
                if (homedatum.Logo != null)
                {
                    string wwwrootPath = _environment.WebRootPath;
                    // delete the old image
                    if (homedatum.Logopath != null)
                    {
                        string oldImage = Path.Combine(wwwrootPath + "/Images/Project/Home/", homedatum.Logopath);

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    // file name
                    string fileName = Guid.NewGuid().ToString() + "_" + homedatum.Logo.FileName;

                    // create path
                    // ~/Images/.....
                    string path = Path.Combine(wwwrootPath + "/Images/Project/Home/", fileName);

                    // add the image to Images folder
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homedatum.Logo.CopyToAsync(fileStream);
                    }
                    homedatum.Logopath = fileName;

                }
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
