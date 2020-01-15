﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool English { get; set; }
        //True = English, False = Welsh
        public string Forename { get; set; }
        public string Surname { get; set; }
    }
}
