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
        public IFormFile UploadStudentModule { set; get; }
        public IFormFile UploadStaff { set; get; }
        public IFormFile UploadModule { set; get; }

        public string CampusCode { get; set; }

        public List<SelectListItem> CampusCodes { get; } = new List<SelectListItem>
    {
        new SelectListItem { Value = "AB0", Text = "Aberystwyth" },
        new SelectListItem { Value = "MU0", Text = "Mauritius" },
        new SelectListItem { Value = "EX1", Text = "External"  },
    };
        public int Year { get; set; }
    }

    //Custom validation
    /*public class FileRequired : ValidationAttribute
    {
        public string _fileName { get; set; }
        public FileRequired(string fileName)
        {
            _fileName = fileName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (FileUploadModel)validationContext.ObjectInstance;
            if (_fileName.Equals("UploadStaff"))
            {
                if (model.UploadStaff == null)
                {
                    return new ValidationResult("CampusCode is required.");
                }
                return ValidationResult.Success;
            }else if (_fileName.Equals("UploadStudent"))
            {
                if(model.UploadStudentModule == null)
                {
                    return new ValidationResult("CampusCode and Year is required.");
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Unknown Error.");
            
        }
    }*/
}
