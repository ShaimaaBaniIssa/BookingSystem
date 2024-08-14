using BookingSystem.Models;
using BookingSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookingSystem.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]

    public class AdminController : Controller
    {

        private readonly ModelContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public AdminController(ModelContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            ViewBag.NumOfRegisteredUsers = _context.AppUsers.Count();

            List<HotelRooms> availableRooms = _context.Rooms.Where(u=>u.Availabilty==1).Include(r=>r.Hotel)
                .GroupBy(u => u.Hotelid).Select(grp => new HotelRooms() { HotelName =grp.First().Hotel.Name , NumOfRooms = grp.Count()}).ToList();

            List<HotelRooms> bookedRooms = _context.Rooms.Where(u => u.Availabilty == 0).Include(r => r.Hotel)
                .GroupBy(u => u.Hotelid).Select(grp => new HotelRooms() { HotelName = grp.First().Hotel.Name, NumOfRooms = grp.Count() }).ToList();

            var tuple = Tuple.Create<List<HotelRooms>, List<HotelRooms>>(availableRooms, bookedRooms);
            return View(tuple);
        }
        public IActionResult Profile()
        {
            var userId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.AppUsers.FirstOrDefault(u => u.Id == userId);

            return View(user);
        }
        public IActionResult EditProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.AppUsers.FirstOrDefault(u => u.Id == userId);

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile([Bind("Id,UserName,FirstName,LastName,Email,PhoneNumber")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _context.AppUsers.FirstOrDefault(u => u.Id == appUser.Id);
                    user.FirstName= appUser.FirstName;
                    user.LastName= appUser.LastName;
                    user.Email= appUser.Email;
                    user.PhoneNumber= appUser.PhoneNumber;

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                   
                        return NotFound();
                   
                }
                return RedirectToAction(nameof(Profile));
            }
            return View(appUser);

        }
    }
}
