using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    /*Sourced from the Webseries guide presented by Kudvenkat, referenced in the Group Report.
     Modified to better fit our needs*/
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Forename { get; set; }
        [Required]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and Confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
