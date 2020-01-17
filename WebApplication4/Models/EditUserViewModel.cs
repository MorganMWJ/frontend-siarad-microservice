using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    /*Sourced from the Webseries guide presented by Kudvenkat, referenced in the Group Report.
     Modified to better fit our needs*/
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<string>();
        }
        [JsonIgnore]
        public string Id { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Forename { get; set; }
        [Required]
        public string Surname { get; set; }
        [JsonIgnore]
        public IList<string> Roles { get; set; }

    }
}
