using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Online_MarketPlace_System.Data;
using Online_MarketPlace_System.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Online_MarketPlace_System.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly MarketplaceDbContext _db;
        private readonly MarketDbContext dblow;

        public HomeController(MarketplaceDbContext db, MarketDbContext db2)
        {
            this._db = db;
            this.dblow = db2;
        }

        public IActionResult Index()
        {

            // HttpContext.Session.SetInt32("Reg_Id", 0);
            if (HttpContext.Session.GetInt32("Reg_Id")!=null)
            {
                int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
                ViewBag.Reg_Id = Reg_Id;
            }
            
            return View();
        }
        public IActionResult Home()
        {
            var productList = _db.Product.ToList();
            ViewData["products"] = productList;
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var otherItems = _db.Product.Where(a => a.User_Id != Reg_Id).ToList();
            ViewData["otherProducts"] = otherItems;
            ViewData["User"] = (int)HttpContext.Session.GetInt32("Reg_Id");
            return View();
        }

        public IActionResult Search(string value)
        {
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var otherItems = _db.Product.Where(a => a.User_Id != Reg_Id).Select(a => a.Id).ToList();
            var items = _db.Product.Where(a => otherItems.Contains(a.Id) && a.Name == value).Select(a => a).ToList();
            ViewData["searchedItems"] = items;
            return View();
        }
        public IActionResult transferProduct(int id)
        {
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var product = _db.Product.Where(a => a.Id == id).Select(a => a).FirstOrDefault();
            var product2 = new Product() { Name = product.Name, Price = product.Price, Image = product.Image, Category = product.Category, Quantity = product.Quantity, Description = product.Description, User_Id = product.User_Id,SecondaryUser=Reg_Id};
            var products = _db.Product.Where(a => a.Name == product2.Name && a.User_Id == product2.User_Id && a.SecondaryUser == product2.SecondaryUser).Select(a => a).ToList();
            
           
            if (products.Count() == 0)
            {
                _db.Add(product2);
                _db.SaveChanges();
            }
            
            return RedirectToAction("Home", "Home");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
