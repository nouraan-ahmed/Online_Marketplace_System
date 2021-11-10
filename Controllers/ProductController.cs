﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<ProductController> _logger;
        private readonly MarketplaceDbContext _db;

        public ProductController(ILogger<ProductController> logger, MarketplaceDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult SelectProduct(int id)
        {
            var product = _db.Product.ToList().Where(p => p.Id == id).FirstOrDefault();
            //var product = _db.Product.Where(p => p.Id == id).ToList().FirstOrDefault();
            ViewData["product"] = product;
            return View();
        }
    }
}
