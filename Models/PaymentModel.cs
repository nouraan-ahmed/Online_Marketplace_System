using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_MarketPlace_System.Models
{
    public class PaymentModel
    {
       
        [Required]
        public int Id { get; set; }
        [Required]
        public string User_Id { get; set; }
        [Required]
        public double Money { get; set; }

        [Required]
        public string CardHolderName { get; set; }

        [Required]

        public int CardNumber { get; set; }

        //[DataType(DataType.Date)]
        [Required]

        public string ExpDate { get; set; }
        [Required]

        public int CVcode { get; set; }

        public int ZipCode { get; set; }


    }
}
