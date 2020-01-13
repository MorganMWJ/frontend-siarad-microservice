using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class ModuleViewModel
    {
        public ModuleViewModel()
        {
        }

        public string Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        public string ClassCode { get; set; }
        
        [Required]
        public string CoordinatorUid { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
