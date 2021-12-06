using Microsoft.EntityFrameworkCore;
using Online_MarketPlace_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_MarketPlace_System.Data
{
    public class MarketDbContext : DbContext
    {
        public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}
