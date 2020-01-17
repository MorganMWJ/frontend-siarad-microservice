using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    /*Sourced from the Webseries guide presented by Kudvenkat, referenced in the Group Report.*/
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
