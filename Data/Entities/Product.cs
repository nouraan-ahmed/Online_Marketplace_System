using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Online_MarketPlace_System.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int? User_Id { get; set; }
        [ForeignKey("User_Id")]
        public virtual User User { get; set; }
        public int Status { get; set; }
        

    }
}
