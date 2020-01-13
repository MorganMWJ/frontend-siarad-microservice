using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class LdapUser
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Uid { get; set; }
        public bool IsStaff { get; set; }

        public bool IsStudent { get; set; }
    }
}
