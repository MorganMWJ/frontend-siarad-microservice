using Newtonsoft.Json;
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
        [JsonIgnore]
        public string Id { get; set; }
        [JsonProperty("Id")]
        private string IdAlternative
        {
            set { Id = value; }
        }
        
        [Required]
        [JsonProperty("code", Order = 1)]
        public string Code { get; set; }
        [JsonProperty("year", Order = 2)]
        [Required]
        public string Year { get; set; }
        [JsonProperty("classCode", Order = 3)]
        [Required]
        public string ClassCode { get; set; }
        [JsonProperty("coordinatorUid", Order = 4)]
        [Required]
        public string CoordinatorUid { get; set; }
        [Required]
        [JsonProperty("title", Order = 5)]
        public string Title { get; set; }
    }
}
