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
        public string Buyer_Name { get; set; }
        [Required]
        public string Seller_Name { get; set; }
        [Required]
        public string Product_Name { get; set; }

        [Required]
        public string Status { get; set; }

    }
}
