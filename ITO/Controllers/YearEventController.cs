using ITO.Models;
using ITO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.Controllers
{
    public class YearEventController : Controller
    {
        private readonly AllContext db;
        public YearEventController(AllContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            return View(await db.YearEvents.Where(ev => ev.Id == id).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            CreateYearEventViewModel model = new CreateYearEventViewModel()
            {
                AgencyId = id,
                Agency = await db.Agencies.FirstOrDefaultAsync(a => a.Id == id),
                Number = await db.YearEvents.Where(y => y.AgencyId == id).CountAsync() + 1,
                Units = await db.Units.ToListAsync(),
                TypeSections = await db.TypeSections.ToListAsync(),
                Sections = await db.Sections.ToListAsync(),
                SubSections = await db.SubSections.ToListAsync(),
                SubSection1s = await db.SubSection1s.ToListAsync(),
                DataYears = await db.DataYears.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateYearEventViewModel model)
        {
            YearEvent yearEvent = new YearEvent()
            {
                AgencyId = model.AgencyId,
                Number = model.Number,
                EventText = model.EventText,
                FirstQuarter = model.FirstQuarter,
                SecondQuarter = model.SecondQuarter,
                ThirdQuarter = model.ThirdQuarter,
                FourthQuarter = model.FourthQuarter,
                Unit = model.Unit,
                TypeSection = model.TypeSection,               
                DataYear = model.DataYear
            };
            Section Section = await db.Sections.FirstOrDefaultAsync(subs => subs.Id == model.SectionId);
            yearEvent.SubSection = Section.Name;

            SubSection subSection = await db.SubSections.FirstOrDefaultAsync(subs => subs.Id == model.SubSectionId);
            yearEvent.SubSection = subSection.Name;

            SubSection1 subSection1 = await db.SubSection1s.FirstOrDefaultAsync(subs => subs.Id == model.SubSection1Id);
            yearEvent.SubSection1 = subSection1.Name;


            db.YearEvents.Add(yearEvent);
            await db.SaveChangesAsync();
            return RedirectToAction("Details","Agency", new { id=yearEvent.AgencyId, DataYearVM = yearEvent.DataYear});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(ye => ye.Id == id);
            if(yearEvent == null)
            {
                return NotFound();
            }
            else
            {
                EditYearEventViewModel model = new EditYearEventViewModel
                {
                    Id = yearEvent.Id,
                    Agency = await db.Agencies.FirstOrDefaultAsync(ag => ag.Id == yearEvent.AgencyId),
                    AgencyId = yearEvent.AgencyId,
                    Number = yearEvent.Number,
                    EventText = yearEvent.EventText,
                    FirstQuarter = yearEvent.FirstQuarter,
                    SecondQuarter = yearEvent.SecondQuarter,
                    ThirdQuarter = yearEvent.ThirdQuarter,
                    FourthQuarter = yearEvent.FourthQuarter,
                    Unit = yearEvent.Unit,
                    Units = await  db.Units.ToListAsync(),
                    TypeSection = yearEvent.TypeSection,
                    TypeSections = await db.TypeSections.ToListAsync(),
                    Section = yearEvent.Section,
                    Sections = await db.Sections.ToListAsync(),

                };
            }

        }
    }
}