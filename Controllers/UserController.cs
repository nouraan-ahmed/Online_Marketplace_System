using Microsoft.AspNetCore.Mvc;
using Online_MarketPlace_System.Data;
using Online_MarketPlace_System.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Online_MarketPlace_System.Controllers
{
    public class UserController : Controller
    {
        private readonly MarketplaceDbContext db;
        private readonly MarketDbContext dblow;
        public UserController(MarketplaceDbContext db,MarketDbContext db2)
        {
            this.db = db;
            this.dblow = db2;
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
            TempData["user_reg_id"] = db.User.Where(o => o.Name == usm.Name).Select(p => p.Id).SingleOrDefault();
            var EmailExist = db.User.ToList().Any(u => u.Email == us.Email);
            var user_id =0;
            if (EmailExist)
            {
                //throw error
                ViewBag.EmailExistError = "You have already signed up";
                //go to error page
                HttpContext.Session.SetInt32("Reg_Id", (int)TempData["user_reg_id"]);
                return Redirect("/User/Error");
            }
            else
            {
                db.Add(us);
                db.SaveChanges();
                user_id = us.Id;
                Payment p = new Payment();
                p.User_Id = user_id;
                p.Money = 100000;
                p.Status = "Credit Card";
                db.Add(p);
                db.SaveChanges();
                HttpContext.Session.SetInt32("Reg_Id", (int)TempData["user_reg_id"]);
                return RedirectToAction("Home", "Home");

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
            var exist = db.User.ToList().Any(i => i.Email == usm.Email);
            HttpContext.Session.SetString("User_Email", usm.Email);
            if (exist)
            {
                 var Data = db.User.Where(f => f.Email == usm.Email).Select(s => new { s.Password, s.Id }).ToList();
                if (BCrypt.Net.BCrypt.Verify(usm.Password, Data[0].Password))
                {
                    User us = new User();
                    HttpContext.Session.SetInt32("Reg_Id", Data[0].Id);
                    //return Redirect("/Home/Index");
                    return RedirectToAction("Home","Home");

                }

            }
            return Redirect("/User/notlog");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.SetInt32("Reg_Id", 0);
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
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            string User_Email = HttpContext.Session.GetString("User_Email");
            var User_id = db.User.Where(p => p.Email == User_Email).Select(o => o.Id).FirstOrDefault();
            string Reg_Name = db.User.Where(l => l.Id == Reg_Id).Select(i => i.Name).FirstOrDefault();
            string Reg_phone = db.User.Where(l => l.Id == Reg_Id).Select(i => i.Phone).FirstOrDefault();
            string Reg_email = db.User.Where(l => l.Id == Reg_Id).Select(i => i.Email).FirstOrDefault();
            ViewData["id_get"] = User_id;
            ViewBag.Id = ViewData["id_get"];
            ViewData["Reg_phone"] = Reg_phone;
            ViewBag.phone = ViewData["Reg_phone"];
            ViewData["Reg_email"] = Reg_email;
            ViewBag.email = ViewData["Reg_email"];

            ViewBag.Name = Reg_Name;
            List<Product> objList = new List<Product>();
            objList = (from s in db.Product select s).Where(p => p.User_Id == Reg_Id).ToList();
            //List<Product> productList = new List<Product>();
            //for (var i = 0; i < objList.Count(); i++)
            //{
            //    ViewData["products"] = objList;
            //    // productList.Add(db.Product.Where(v => v.Id == objList[i]).FirstOrDefault());
            //}
            ViewData["products"] = objList;
            // ViewData["products"] = productList;
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(ProductModel entity)
        {
            string User_Email = HttpContext.Session.GetString("User_Email");
            int User_id = db.User.Where(p => p.Email == User_Email).Select(o => o.Id).FirstOrDefault();
            ViewData["id"] = User_id;
            Product pro = new Product();
            pro.Name = entity.Name;
            pro.Price = entity.Price;
            pro.Quantity = entity.Quantity;
            pro.Description = entity.Description;
            pro.Category = entity.Category;
            pro.Id = entity.Id;
            pro.Image = "https://www.lg.com/lg5-common/images/common/product-default-list-350.jpg";
            pro.User_Id = db.User.Where(p => p.Id == User_id).Select(o => o.Id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                db.Add(pro);
                db.SaveChanges();

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

            var instance = db.Product.Find(id);
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
                db.Update(pro);
                db.SaveChanges();
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

            var instance = db.Product.Find(id);
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
            var obj = db.Product.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                db.Remove(obj);
                db.SaveChanges();
                return RedirectToAction("Profile");
            }

        }

    }
}
