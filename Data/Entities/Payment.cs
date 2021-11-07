using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Online_MarketPlace_System.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public int? User_Id { get; set; }
        [ForeignKey("User_Id")]
        public virtual User User { get; set; }
        public double Money { get; set; }
        public string Status { get; set; }
    }
}
