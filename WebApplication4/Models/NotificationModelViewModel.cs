using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class NotificationModelViewModel
    {
        [JsonProperty("uid", Order = 1)]
        public String Uid { get; set; }
        [Required]
        [JsonProperty("daily", Order = 2)]
        public bool Daily { get; set; }
        [Required]
        [JsonProperty("mentions", Order = 3)]
        public bool Mentions { get; set; }
        [Required]
        [JsonProperty("replies", Order = 4)]
        public bool Replies { get; set; }
        [Required]
        [Range(0,24)]
        [JsonProperty("notificationinterval", Order = 5)]
        public int NotificationInterval { get; set; }
        [Required]
        [JsonProperty("lastupdated", Order = 6)]
        public DateTime LastUpdated { get; set; }
    }
}
