using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Online_MarketPlace_System.Data;
using Online_MarketPlace_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_MarketPlace_System.Controllers
{
 
    public class ProductController : Controller
    {
        private readonly MarketplaceDbContext _db;
        private readonly MarketDbContext dblow;

        public ProductController(MarketplaceDbContext db,MarketDbContext db2)
        {
            this._db = db;
            this.dblow = db2;
        }
        public IActionResult SelectProduct(int id)
        {
            var product = _db.Product.ToList().Where(p => p.Id == id).FirstOrDefault();
            //var product = _db.Product.Where(p => p.Id == id).ToList().FirstOrDefault();
            ViewData["product"] = product;
            return View();
        }

        public IActionResult AddToCart(int id)
        {
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var product = _db.Product.ToList().Where(p => p.Id == id).FirstOrDefault();
            Transaction tr = new Transaction();
            tr.Seller_Id =(int) product.User_Id;
            tr.User_Id= (int)HttpContext.Session.GetInt32("Reg_Id");
            tr.Status = "Pending";
            tr.Product_Id = id;
            _db.Add(tr);
            _db.SaveChanges();
            
            Product p = new Product();
            p = _db.Product.FirstOrDefault(s => s.Id == id);
            if( p.User_Id == tr.Seller_Id)
            {
                p.Status = 1;
                _db.Update(p);
                _db.SaveChanges();
            }
            Product pro = new Product();
            pro.Name = _db.Product.Where(o=>o.Id== id).Select(p=>p.Name).FirstOrDefault();
            pro.Price = _db.Product.Where(o => o.Id == id).Select(p => p.Price).FirstOrDefault();
            pro.Quantity = 1;
            pro.Description = _db.Product.Where(o => o.Id == id).Select(p => p.Description).FirstOrDefault();
            pro.Category = _db.Product.Where(o => o.Id == id).Select(p => p.Category).FirstOrDefault();
           // pro.Id = id;
            pro.Image = "https://www.lg.com/lg5-common/images/common/product-default-list-350.jpg";
            pro.User_Id = Reg_Id;

            _db.Add(pro);
            _db.SaveChanges();

            return RedirectToAction("Profile","User");
        }
    }
}
