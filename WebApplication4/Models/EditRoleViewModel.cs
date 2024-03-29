﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    /*Sourced from the Webseries guide presented by Kudvenkat, referenced in the Group Report.
     Modified to better fit our needs*/
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<String>();
        }
        public string Id { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }

    }
}
