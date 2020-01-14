using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class FileUploadModel
    {
        [Required]
        public IFormFile UploadStudentModule { set; get; }
        [Required]
        public IFormFile UploadStaff { set; get; }
        [Required]
        public IFormFile UploadModule { set; get; }

        [Required]
        public string CampusCode { get; set; }
        [Required]

        public List<SelectListItem> CampusCodes { get; } = new List<SelectListItem>
    {
        new SelectListItem { Value = "AB0", Text = "Aberystwyth" },
        new SelectListItem { Value = "MU0", Text = "Mauritius" },
        new SelectListItem { Value = "EX1", Text = "External"  },
    };
        [Range(1900,2099)]
        [Required]
        public int Year { get; set; }
    }
}
