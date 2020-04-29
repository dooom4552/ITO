using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<YearEvent> YearEvents { get; set; }
        public DbSet<PartYearEvent> PartYearEvents { get; set; }
        public DbSet<Unit> Units { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();     
        }
    }
}
