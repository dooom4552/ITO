using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.Models
{
    public class AllContext : DbContext
    {
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<YearEvent> YearEvents { get; set; }
        public DbSet<PartYearEvent> PartYearEvents { get; set; }
        public DbSet<Unit> Units { get; set; }
        public AllContext(DbContextOptions<AllContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Unit>().HasData(
                new Unit[]
            {
                new Unit{Id=1, Name="п.м."},
                new Unit{Id=2, Name="шт."},
                new Unit{Id=3, Name="к-т."}
            }
             );
        }
    }
}
