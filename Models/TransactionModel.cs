using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_MarketPlace_System.Models
{
    public class TransactionModel
    {
       
        [Required]
        public int Id { get; set; }
        [Required]
        public int User_Id { get; set; }
        [Required]
        public int Seller_Id { get; set; }
        [Required]
        public int Product_Id { get; set; }

        [Required]
        public string Status { get; set; }

    }
}
