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
    public class AboutusdatumsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;


        public AboutusdatumsController(ModelContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Aboutusdatums
        public async Task<IActionResult> Index()
        {
              return _context.Aboutusdata != null ? 
                          View(await _context.Aboutusdata.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Aboutusdata'  is null.");
        }

        // GET: Aboutusdatums/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Aboutusdata == null)
            {
                return NotFound();
            }

            var aboutusdatum = await _context.Aboutusdata
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutusdatum == null)
            {
                return NotFound();
            }

            return View(aboutusdatum);
        }

        // GET: Aboutusdatums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aboutusdatums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ImageFile1,ImageFile2,ImageFile3,ImageFile4,ImgTitle1,ImgTitle2,ImgTitle3,ImgTitle4")] Aboutusdatum aboutusdatum)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _environment.WebRootPath;
                
                if (aboutusdatum.ImageFile1 != null)
                {

                    string fileName = Guid.NewGuid().ToString() + "_" + aboutusdatum.ImageFile1.FileName;

                    string path = Path.Combine(wwwrootPath + "/Images/Project/AboutUs/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await aboutusdatum.ImageFile1.CopyToAsync(fileStream);
                    }
                    aboutusdatum.Imgpath1 = fileName;

                }
                if (aboutusdatum.ImageFile2 != null)
                {

                    string fileName = Guid.NewGuid().ToString() + "_" + aboutusdatum.ImageFile2.FileName;

                    string path = Path.Combine(wwwrootPath + "/Images/Project/AboutUs/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await aboutusdatum.ImageFile2.CopyToAsync(fileStream);
                    }
                    aboutusdatum.Imgpath2 = fileName;

                }
                if (aboutusdatum.ImageFile3 != null)
                {

                    string fileName = Guid.NewGuid().ToString() + "_" + aboutusdatum.ImageFile3.FileName;

                    string path = Path.Combine(wwwrootPath + "/Images/Project/AboutUs/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await aboutusdatum.ImageFile3.CopyToAsync(fileStream);
                    }
                    aboutusdatum.Imgpath3 = fileName;

                }
                if (aboutusdatum.ImageFile4 != null)
                {

                    string fileName = Guid.NewGuid().ToString() + "_" + aboutusdatum.ImageFile4.FileName;

                    string path = Path.Combine(wwwrootPath + "/Images/Project/AboutUs/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await aboutusdatum.ImageFile4.CopyToAsync(fileStream);
                    }
                    aboutusdatum.Imgpath4 = fileName;

                }
                _context.Add(aboutusdatum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutusdatum);
        }

        // GET: Aboutusdatums/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Aboutusdata == null)
            {
                return NotFound();
            }

            var aboutusdatum = await _context.Aboutusdata.FindAsync(id);
            if (aboutusdatum == null)
            {
                return NotFound();
            }
            return View(aboutusdatum);
        }

        // POST: Aboutusdatums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Title,Description,ImageFile1,ImageFile2,ImageFile3,ImageFile4,Imgpath1,Imgpath2,Imgpath3,Imgpath4,ImgTitle1,ImgTitle2,ImgTitle3,ImgTitle4")] Aboutusdatum aboutusdatum)
        {
           
            if (ModelState.IsValid)
            {
                if (aboutusdatum.ImageFile1 != null)
                {
                    string wwwrootPath = _environment.WebRootPath;
                    // delete the old image
                    if (aboutusdatum.Imgpath1 != null)
                    {
                        string oldImage = Path.Combine(wwwrootPath + "/Images/Project/AboutUs/", aboutusdatum.Imgpath1);

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    // file name
                    string fileName = Guid.NewGuid().ToString() + "_" + aboutusdatum.ImageFile1.FileName;

                    // create path
                    // ~/Images/.....
                    string path = Path.Combine(wwwrootPath + "/Images/Project/AboutUs/", fileName);

                    // add the image to Images folder
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await aboutusdatum.ImageFile1.CopyToAsync(fileStream);
                    }
                    aboutusdatum.Imgpath1 = fileName;

                }
                if (aboutusdatum.ImageFile2 != null)
                {
                    string wwwrootPath = _environment.WebRootPath;
                    // delete the old image
                    if (aboutusdatum.Imgpath2 != null)
                    {
                        string oldImage = Path.Combine(wwwrootPath + "/Images/Project/AboutUs/", aboutusdatum.Imgpath2);

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    // file name
                    string fileName = Guid.NewGuid().ToString() + "_" + aboutusdatum.ImageFile2.FileName;
    
                    // create path
                    // ~/Images/.....
                    string path = Path.Combine(wwwrootPath + "/Images/Project/AboutUs/", fileName);

                    // add the image to Images folder
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await aboutusdatum.ImageFile2.CopyToAsync(fileStream);
                    }
                    aboutusdatum.Imgpath2 = fileName;

                }
                if (aboutusdatum.ImageFile3 != null)
                {
                    string wwwrootPath = _environment.WebRootPath;
                    // delete the old image
                    if (aboutusdatum.Imgpath3 != null)
                    {
                        string oldImage = Path.Combine(wwwrootPath + "/Images/Project/AboutUs/", aboutusdatum.Imgpath3);

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    // file name
                    string fileName = Guid.NewGuid().ToString() + "_" + aboutusdatum.ImageFile3.FileName;

                    // create path
                    // ~/Images/.....
                    string path = Path.Combine(wwwrootPath + "/Images/Project/AboutUs/", fileName);

                    // add the image to Images folder
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await aboutusdatum.ImageFile3.CopyToAsync(fileStream);
                    }
                    aboutusdatum.Imgpath3 = fileName;

                }
                if (aboutusdatum.ImageFile4 != null)
                {
                    string wwwrootPath = _environment.WebRootPath;
                    // delete the old image
                    if (aboutusdatum.Imgpath4 != null)
                    {
                        string oldImage = Path.Combine(wwwrootPath + "/Images/Project/AboutUs/", aboutusdatum.Imgpath4);

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    // file name
                    string fileName = Guid.NewGuid().ToString() + "_" + aboutusdatum.ImageFile4.FileName;

                    // create path
                    // ~/Images/.....
                    string path = Path.Combine(wwwrootPath + "/Images/Project/AboutUs/", fileName);

                    // add the image to Images folder
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await aboutusdatum.ImageFile4.CopyToAsync(fileStream);
                    }
                    aboutusdatum.Imgpath4 = fileName;

                }
                try
                {
                    _context.Update(aboutusdatum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutusdatumExists(aboutusdatum.Id))
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
            return View(aboutusdatum);
        }

        // GET: Aboutusdatums/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Aboutusdata == null)
            {
                return NotFound();
            }

            var aboutusdatum = await _context.Aboutusdata
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutusdatum == null)
            {
                return NotFound();
            }

            return View(aboutusdatum);
        }

        // POST: Aboutusdatums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Aboutusdata == null)
            {
                return Problem("Entity set 'ModelContext.Aboutusdata'  is null.");
            }
            var aboutusdatum = await _context.Aboutusdata.FindAsync(id);
            if (aboutusdatum != null)
            {
                _context.Aboutusdata.Remove(aboutusdatum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutusdatumExists(decimal id)
        {
          return (_context.Aboutusdata?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
