using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class FileExportModel
    {
        [JsonProperty("module_code", Order=1)]
        public string module_code { get; set; }
        [JsonProperty("mod_full_name", Order = 2)]
        public string mod_full_name { get; set; }
        [JsonProperty("academic_year", Order = 3)]
        public string academic_year { get; set; }
        [JsonProperty("email", Order = 4)]
        public string email { get; set; }
        [JsonProperty("name", Order = 5)]
        public string name { get; set; }
    }
}
