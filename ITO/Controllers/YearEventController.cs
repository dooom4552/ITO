using ITO.Models;
using ITO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.Controllers
{
    [Authorize(Roles = "admin")]
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
                Number = await db.YearEvents.Where(y => y.AgencyId == model.AgencyId && y.DataYear == model.DataYear).CountAsync()+1,
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
            yearEvent.Section = Section.Name;

            SubSection subSection = await db.SubSections.FirstOrDefaultAsync(subs => subs.Id == model.SubSectionId);
            yearEvent.SubSection = subSection.Name;

            SubSection1 subSection1 = await db.SubSection1s.FirstOrDefaultAsync(subs => subs.Id == model.SubSection1Id);
            yearEvent.SubSection1 = subSection1.Name;


            db.YearEvents.Add(yearEvent);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Agency", new { id = yearEvent.AgencyId, DataYearVM = yearEvent.DataYear });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(ye => ye.Id == id);
            if (yearEvent == null)
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
                    Units = await db.Units.ToListAsync(),
                    TypeSection = yearEvent.TypeSection,
                    TypeSections = await db.TypeSections.ToListAsync(),
                    Sections = await db.Sections.ToListAsync(),
                    SubSections = await db.SubSections.ToListAsync(),
                    SubSection = yearEvent.SubSection,
                    SubSection1s = await db.SubSection1s.ToListAsync(),
                    SubSection1 = yearEvent.SubSection1,
                    DataYear = yearEvent.DataYear,
                    DataYears = await db.DataYears.ToListAsync()
                };
                if (yearEvent.Section == null)
                {
                    yearEvent.Section = "ИСО";
                }
                Section section = await db.Sections.FirstOrDefaultAsync(sec => sec.Name == yearEvent.Section);

                model.SectionId = section.Id;
                SubSection subSection = await db.SubSections.FirstOrDefaultAsync(sub => sub.Name == yearEvent.SubSection);
                model.SubSectionId = subSection.Id;
                SubSection1 subSection1 = await db.SubSection1s.FirstOrDefaultAsync(sub1 => sub1.Name == yearEvent.SubSection1);
                model.SubSection1Id = subSection1.Id;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditYearEventViewModel model)
        {
            YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(ye => ye.Id == model.Id);
            yearEvent.EventText = model.EventText;
            yearEvent.FirstQuarter = model.FirstQuarter;
            yearEvent.SecondQuarter = model.SecondQuarter;
            yearEvent.ThirdQuarter = model.ThirdQuarter;
            yearEvent.FourthQuarter = model.FourthQuarter;
            yearEvent.Unit = model.Unit;
            yearEvent.TypeSection = model.TypeSection;
            Section section = await db.Sections.FirstOrDefaultAsync(sec => sec.Id == model.SectionId);
            yearEvent.Section = section.Name;
            SubSection subSection = await db.SubSections.FirstOrDefaultAsync(sub => sub.Id == model.SubSectionId);
            yearEvent.SubSection = subSection.Name;
            SubSection1 subSection1 = await db.SubSection1s.FirstOrDefaultAsync(sub1 => sub1.Id == model.SubSection1Id);
            yearEvent.SubSection1 = subSection1.Name;
            yearEvent.DataYear = model.DataYear;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Agency", new { id = yearEvent.AgencyId, DataYearVM = yearEvent.DataYear });
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(ye => ye.Id == id);
                if (yearEvent != null)
                {
                    DeleteYearEventViewModel model = new DeleteYearEventViewModel()
                    {
                        Id = (int)id,
                        AgencyId = yearEvent.AgencyId,
                        Number = yearEvent.Number,
                        EventText = yearEvent.EventText,
                        FirstQuarter = yearEvent.FirstQuarter,
                        SecondQuarter = yearEvent.SecondQuarter,
                        ThirdQuarter = yearEvent.ThirdQuarter,
                        FourthQuarter = yearEvent.FourthQuarter,
                        Unit = yearEvent.Unit,
                        Section = yearEvent.Section,
                        SubSection = yearEvent.SubSection,
                        SubSection1 = yearEvent.SubSection1,
                        TypeSection = yearEvent.TypeSection,
                        DataYear = yearEvent.DataYear,
                        partYearEvents = await db.PartYearEvents.Where(p => p.YearEventId == yearEvent.Id).ToListAsync()
                    };
                    return View(model);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(ye => ye.Id == id);
            if (yearEvent != null)
            {
                db.YearEvents.Remove(yearEvent);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Agency", new { id = yearEvent.AgencyId, DataYearVM = yearEvent.DataYear });
            }
            return NotFound();
        }
    }
}