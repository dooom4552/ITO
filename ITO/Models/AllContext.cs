using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITO.ViewModels.AgencyUser;

namespace ITO.Models
{
    public class AllContext : DbContext
    {
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<YearEvent> YearEvents { get; set; }
        public DbSet<PartYearEvent> PartYearEvents { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<TypeSection> TypeSections { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SubSection> SubSections { get; set; }
        public DbSet<SubSection1> SubSection1s { get; set; }
        public DbSet<DataYear> DataYears { get; set; }
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
            modelBuilder.Entity<TypeSection>().HasData(
                new TypeSection[]
             {
                 new TypeSection{Id=1, Name="ремонт"},
                 new TypeSection{Id=2, Name="кап.ремонт"},
                 new TypeSection{Id=3, Name="установлено"},
                 new TypeSection{Id=4, Name="демонтировано"}
             }
             );
            modelBuilder.Entity<Section>().HasData(
                new Section[]
             {
                 new Section{Id=1, Name="ТСОН"},
                 new Section{Id=2, Name="ИСО"}
             }
             );
            modelBuilder.Entity<SubSection>().HasData(
                new SubSection[]
             {
                 new SubSection{Id=1, SectionId=1, Name="Видео"},
                 new SubSection{Id=2, SectionId=1, Name="ОИ"},
                 new SubSection{Id=3, SectionId=1, Name="Кабель"},
                 new SubSection{Id=4, SectionId=1, Name="ИСБ"},
                 new SubSection{Id=5, SectionId=2, Name="Ограждение"},
                 new SubSection{Id=6, SectionId=2, Name="ППК"},
                 new SubSection{Id=7, SectionId=2, Name="ППЗ"}
             }
             );
            modelBuilder.Entity<SubSection1>().HasData(
                new SubSection1[]
             {
                    new SubSection1{ Id=1, SubSectionId=1, Name="Видеокамера внутр"},
                    new SubSection1{ Id=2, SubSectionId=1, Name="Видеокамера уличн"},
                    new SubSection1{ Id=3, SubSectionId=1, Name="Видеорегистратор стационарный"},
                    new SubSection1{ Id=4, SubSectionId=1, Name="ПВР"},
                    new SubSection1{ Id=5, SubSectionId=1, Name="Приемник-передатчик"},
                    new SubSection1{ Id=6, SubSectionId=2, Name="РВОИ 2 поз"},
                    new SubSection1{ Id=7, SubSectionId=2, Name="РВОИ 1 поз"},
                    new SubSection1{ Id=8, SubSectionId=2, Name="ТВОИ"},
                    new SubSection1{ Id=9, SubSectionId=2, Name="ОЭОИ 2 поз"},
                    new SubSection1{ Id=10, SubSectionId=2, Name="ОЭОИ 1 поз"},
                    new SubSection1{ Id=11, SubSectionId=2, Name="ПВОИ"},
                    new SubSection1{ Id=12, SubSectionId=2, Name="ВОИ"},
                    new SubSection1{ Id=13, SubSectionId=2, Name="комб ОИ"},
                    new SubSection1{ Id=14, SubSectionId=3, Name="Сигнальный"},
                    new SubSection1{ Id=15, SubSectionId=3, Name="Питающий"},
                    new SubSection1{ Id=16, SubSectionId=4, Name="АРМ"},
                    new SubSection1{ Id=17, SubSectionId=4, Name="Другое"},
                    new SubSection1{ Id=18, SubSectionId=5, Name="ОО"},
                    new SubSection1{ Id=19, SubSectionId=5, Name="ВНЕШНЗЗ"},
                    new SubSection1{ Id=20, SubSectionId=5, Name="ВНУТРЗЗ"},
                    new SubSection1{ Id=21, SubSectionId=5, Name="ЭО"},
                    new SubSection1{ Id=22, SubSectionId=5, Name="ПРЕДУПР"},
                    new SubSection1{ Id=23, SubSectionId=5, Name="ЛОКАЛН"},
                    new SubSection1{ Id=24, SubSectionId=6, Name="АСКЛ Егоза"},
                    new SubSection1{ Id=25, SubSectionId=6, Name="Другое"},
                    new SubSection1{ Id=26, SubSectionId=7, Name="ШИПОВНИК"},
                    new SubSection1{ Id=27, SubSectionId=7, Name="Другое"}
             }
             );
            modelBuilder.Entity<DataYear>().HasData(
                new DataYear[]
             {
                 new DataYear{Id=1, Name="2020"},
                 new DataYear{Id=2, Name="2021"},
                 new DataYear{Id=3, Name="2022"},
             }
             );
        }
    }
}
