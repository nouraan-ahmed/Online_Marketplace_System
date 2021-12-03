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
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Online_MarketPlace_System.Models;

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
        public IActionResult Success()
        {
            return View();
        }

       

        [HttpGet]
        public IActionResult Part()
        {
            return View();
        }
        public IActionResult User_profile()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
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
            TempData["user_reg_id"] = _db.User.Where(o => o.Name == usm.Name).Select(p => p.Id).Single();
            var EmailExist = _db.User.ToList().Any(u => u.Email == us.Email);
            
            if (EmailExist)
            {
                //throw error
                ViewBag.EmailExistError = "You have already signed up";
                //go to error page
                HttpContext.Session.SetInt32("User_Reg_Id", (int)TempData["user_reg_id"]);
                return Redirect("/User/Error");
            }
            else
            {
                _db.Add(us);
                _db.SaveChanges();
                HttpContext.Session.SetInt32("User_Reg_Id", (int)TempData["user_reg_id"]);
                return RedirectToAction("Success");

            }

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
            HttpContext.Session.SetString("User_Email", usm.Email);
            if (exist)
            {
                 var Data = _db.User.Where(f => f.Email == usm.Email).Select(s => new { s.Password, s.Id }).ToList();
                if (BCrypt.Net.BCrypt.Verify(usm.Password, Data[0].Password))
                {
                    User us = new User();

                    return RedirectToAction("Profile");

                }

            }
            return Redirect("/User/notlog");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.SetInt32("User_Reg_Id", 0);
            HttpContext.Session.SetString("User_Email", null);
            return Redirect("/Home/Index");
        }
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Profile(User usm)
        {
            string User_Email = HttpContext.Session.GetString("User_Email");
            var User_id = _db.User.Where(p => p.Email == User_Email).Select(o => o.Id).FirstOrDefault();
            ViewData["id_get"] = User_id;
            ViewBag.Id = ViewData["id_get"];

            IEnumerable<Product> objList = _db.Product;
            return View(objList);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(ProductModel entity)
        {
            string User_Email = HttpContext.Session.GetString("User_Email");
            int User_id = _db.User.Where(p => p.Email == User_Email).Select(o => o.Id).FirstOrDefault();
            ViewData["id"] = User_id;
            Product pro = new Product();
            pro.Name = entity.Name;
            pro.Price = entity.Price;
            pro.Quantity = entity.Quantity;
            pro.Description = entity.Description;
            pro.Category = entity.Category;
            pro.Id = entity.Id;
            pro.User_Id = _db.User.Where(p => p.Id == User_id).Select(o => o.Id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                _db.Add(pro);
                _db.SaveChanges();
               
                return RedirectToAction("Profile");
            }


            return View(entity);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var instance = _db.Product.Find(id);
            if (instance == null)
            {
                return NotFound();
            }

            return View(instance);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductModel obj)
        {
            Product pro = new Product
            {
                Name = obj.Name,
                Price = obj.Price,
                Quantity = obj.Quantity,
                Description = obj.Description,
                Category = obj.Category,
                Id = obj.Id,
            };
            if (ModelState.IsValid)
            {
                _db.Update(pro);
                _db.SaveChanges();
                return RedirectToAction("Profile");
            }

            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var instance = _db.Product.Find(id);
            // Product instance = _db.Product.Include(u => u.Quantity).Include(u => u.Price).FirstOrDefault(u => u.Id == id);

            if (instance == null)
            {
                return NotFound();
            }

            return View(instance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Deleteconfirm(int? id)
        {
            var obj = _db.Product.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                _db.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Profile");
            }

        }

    }
}
