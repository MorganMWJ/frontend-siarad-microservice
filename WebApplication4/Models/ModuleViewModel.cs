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
        //[JsonIgnore]
        [JsonProperty("id", Order = 1)]
        public int Id { get; set; }
        //[JsonProperty("Id")]
        //private string IdAlternative
        //{
        //    set { Id = value; }
        //}
        
        [Required]
        [JsonProperty("code", Order = 2)]
        public string Code { get; set; }
        [JsonProperty("year", Order = 3)]
        [Required]
        public string Year { get; set; }
        [JsonProperty("classCode", Order = 4)]
        [Required]
        public string ClassCode { get; set; }
        [JsonProperty("coordinatorUid", Order = 5)]
        [Required]
        public string CoordinatorUid { get; set; }
        [Required]
        [JsonProperty("title", Order = 6)]
        public string Title { get; set; }

        [JsonIgnore]
        public List<Group> Groups { get; set; }
    }
}
