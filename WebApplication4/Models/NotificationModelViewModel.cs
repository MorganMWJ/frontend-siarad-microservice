using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class NotificationModelViewModel
    {
        public String Uid { get; set; }
        public bool Daily { get; set; }
        public bool Mentions { get; set; }
        public bool Replies { get; set; }
        public int NotificationInterval { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
