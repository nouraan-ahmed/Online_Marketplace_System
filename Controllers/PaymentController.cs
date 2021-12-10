using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_MarketPlace_System.Data;
using Online_MarketPlace_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_MarketPlace_System.Controllers
{
    public class PaymentController : Controller
    {
        private readonly MarketplaceDbContext _db;
        private readonly MarketDbContext dblow;
        public PaymentController(MarketplaceDbContext db,MarketDbContext db2)
        {
           this._db = db;
            this.dblow = db2;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Payment()
        {
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var products = (from p in _db.Transaction select p).Where(f => f.User_Id == Reg_Id).Select(h => h.Product_Id).ToList();

            double userMoney = _db.Payment.Where(v => v.Id == Reg_Id).Select(d => d.Money).FirstOrDefault();
            double totalMoney = 0;
            for (var i = 0; i < products.Count(); i++)
            {
                totalMoney += _db.Product.Where(v => v.Id == products[i]).Select(d => d.Price).FirstOrDefault();
            }
            ViewBag.massage = totalMoney;


            return View();
        }
        [HttpPost]
        public IActionResult Payment(PaymentVM pvm)
        {
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            var products = (from p in _db.Transaction select p).Where(f => f.User_Id == Reg_Id).Select(h => h.Product_Id).ToList();
            double userMoney =_db.Payment.Where(v => v.Id == Reg_Id).Select(d => d.Money).FirstOrDefault();
            double totalMoney = 0;
            for (var i = 0; i < products.Count(); i++)
            {
                totalMoney += _db.Product.Where(v => v.Id == products[i]).Select(d => d.Price).FirstOrDefault();
            }
            //ViewBag.massage = totalMoney;
            Payment pp;
            pp = _db.Payment.FirstOrDefault(s => s.User_Id == Reg_Id);
            pp.Money = userMoney-totalMoney;
            _db.Update(pp);
            _db.SaveChanges();

            return RedirectToAction("savepay", "Payment");
        }
        [HttpGet]
        public IActionResult savepay()
        {
            return View();
        }

        [HttpPost]
        public IActionResult savepay(savepayVM svm)
        {
            Transaction tr;
            int Reg_Id = (int)HttpContext.Session.GetInt32("Reg_Id");
            //tr = _db.Transaction.FirstOrDefault(s => s.User_Id == Reg_Id);
            var objList = _db.Transaction.Where(p => p.User_Id == Reg_Id).ToList();

            var productList = new List<Transaction>();
          
            for (var i = 0; i < objList.Count(); i++)
            {
                tr=_db.Transaction.FirstOrDefault(s => s.Status=="Pending");
                tr.Status = "Done";
               
                _db.Update(tr);
                _db.SaveChanges();
            }
            
            return View();
        }
    }
}
