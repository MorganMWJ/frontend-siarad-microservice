using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class StaffAndStudentModel
    {
        [JsonProperty("uid", Order = 1)]
        public string Uid { get; set; }
        [JsonProperty("forename", Order = 2)]
        public string Forename { get; set; }
        [JsonProperty("surname", Order = 3)]
        public string Surname { get; set; }

    }
}
