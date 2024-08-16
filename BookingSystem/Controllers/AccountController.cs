using BookingSystem.Models;
using BookingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelContext _context;
        private readonly IPasswordHasher _passwordHasher;


        public AccountController(ModelContext context,IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register([Bind("Customerid,Firstname,Lastname,Email,Phonenumber")] Customer customer,
            string username, string password)
        {
            if (ModelState.IsValid)
            {
                
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                var hashedPass = _passwordHasher.Hash(password);
                _context.UserLogins.Add(new UserLogin { Username = username, Hashedpassword = hashedPass, Customerid = customer.Customerid, Roleid = 1 });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            TempData["error"] = "Invalid data";

            return View(customer);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username,Hashedpassword")] UserLogin userlogin)
        {
            
                var user = _context.UserLogins
                    .Include(u => u.Customer)
                .SingleOrDefault(m => m.Username == userlogin.Username);

                var result = _passwordHasher.Verify(user.Hashedpassword, userlogin.Hashedpassword);
                if (result)
                {

                    if (user != null)
                    {

                        if (user.Roleid == 1)
                        {
                            //set session
                            HttpContext.Session.SetString("Name", user.Customer.Firstname);
                        HttpContext.Session.SetInt32("Id", (int)user.Customerid);

                        HttpContext.Session.SetInt32("RoleId", (int)user.Roleid);
                            return RedirectToAction("Index", "Home");
                        }
                        else if (user.Roleid == 2)
                        {
                            HttpContext.Session.SetInt32("RoleId", (int)user.Roleid);
                        HttpContext.Session.SetInt32("Id", (int)user.Id);

                        HttpContext.Session.SetString("Name", user.Customer.Firstname);

                            return RedirectToAction("Index", "Admin");
                        }

                    }
                }
            TempData["error"] = "Invalid username or password";

            return View(userlogin);

            
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
