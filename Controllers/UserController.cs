using Microsoft.AspNetCore.Mvc;
using Online_MarketPlace_System.Data;
using Online_MarketPlace_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_MarketPlace_System.Controllers
{
    public class UserController : Controller
    {
        private readonly MarketplaceDbContext _db;
        public UserController(MarketplaceDbContext db)
        {
            _db = db;
        }
        public IActionResult Register()
        {
            //IEnumerable<User> userList = _db.User;
            return View();
        }
    }
}
