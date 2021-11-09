﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_MarketPlace_System.Models
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MinLength(4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
