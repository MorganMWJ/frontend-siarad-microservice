using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Data
{
    public class FrontendContext : DbContext
    {     
            public FrontendContext(DbContextOptions<FrontendContext> options) : base(options) { }

            public DbSet<Group> Groups { get; set; }
        }
}
