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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
