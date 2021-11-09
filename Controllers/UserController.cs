using Microsoft.AspNetCore.Mvc;
using Online_MarketPlace_System.Data;
using Online_MarketPlace_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Online_MarketPlace_System.Controllers
{
    public class UserController : Controller
    {
        private readonly MarketplaceDbContext _db;
        public UserController(MarketplaceDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Register()
        {
            //IEnumerable<User> userList = _db.User;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserModel usm)
        {
            User us = new User
            {
                Name = usm.Name,
                Email = usm.Email,
                Phone = usm.Phone,
                Password = BCrypt.Net.BCrypt.HashPassword(usm.Password)
            };
            var EmailExist = _db.User.ToList().Any(u => u.Email == us.Email);
            if (EmailExist)
            {
                //throw error
                ViewBag.EmailExistError = "You have already signed up";
                //go to error page
                return Redirect("Home");
            }
            else
            {
                _db.Add(us);
                _db.SaveChanges();

            }
            HttpContext.Session.SetInt32("User_Reg_Id", (int)us.Id);
            return Redirect("Home/Regestration_success");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult NotLog()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel usm)
        {
            var exist = _db.User.ToList().Any(i => i.Email == usm.Email);
            if (exist)
            {
                var Data = _db.User.Where(f => f.Email == usm.Email).Select(s => new { s.Password, s.Id }).ToList();
                if (BCrypt.Net.BCrypt.Verify(usm.Password, Data[0].Password))
                {
                    User us = new User();
                    HttpContext.Session.SetInt32("User_Reg_Id", Data[0].Id);
               
                }
            }
            return Redirect("/User/profileUser");
        }
    }
}
