using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITO.Models;
using ITO.ViewModels;
using ITO.ViewModels.Sections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITO.Controllers
{
    [Authorize(Roles = "admin")]
    public class SectionController : Controller
    {
        private readonly AllContext db;
        public SectionController(AllContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            EditSectionViewModel model = new EditSectionViewModel()
            {
                Units=await db.Units.ToListAsync(),
                TypeSections= await db.TypeSections.ToListAsync(),
                Sections= await db.Sections.ToListAsync(),
                SubSections= await db.SubSections.ToListAsync(),
                SubSection1s= await db.SubSection1s.ToListAsync(),
                DataYears = await db.DataYears.ToListAsync(),
            };
            return View(model);
        }
        [HttpGet]
        [ActionName("DeleteUnit")]
        public async Task<IActionResult> ConfirmDeleteUnit(int? id)
        {
            if(id != null)
            {
                Unit unit = await db.Units.FirstOrDefaultAsync(u => u.Id == id);
                if(unit != null)
                {
                    UnitViewModel model = new UnitViewModel()
                    {
                        Id = (int)id,
                        Name = unit.Name,
                        YearEvents = await db.YearEvents.Where(ye => ye.Unit == unit.Name).ToListAsync()
                    };
                    return View(model);

                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUnit(int? id)
        {
            if(id != null)
            {
                Unit unit = await db.Units.FirstOrDefaultAsync(u => u.Id == id);
                if(unit != null)
                {
                     db.Units.Remove(unit);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUnit(EditSectionViewModel model)
        {
            Unit unit = await db.Units.FirstOrDefaultAsync(u => u.Name == model.Name);
            
            if (unit == null)
            {
                Unit _unit = new Unit()
                {
                    Name=model.Name
                };
                db.Units.Add(_unit);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        
        [HttpGet]
        [ActionName("DeleteTypeSection")]
        public async Task<IActionResult> ConfirmDeleteTypeSection(int? id)
        {
            if(id != null)
            {
                TypeSection typeSection = await db.TypeSections.FirstOrDefaultAsync(u => u.Id == id);
                if(typeSection != null)
                {
                    TypeSectionViewModel model = new TypeSectionViewModel()
                    {
                        Id = (int)id,
                        Name = typeSection.Name,
                        YearEvents = await db.YearEvents.Where(ye => ye.TypeSection == typeSection.Name).ToListAsync()
                    };
                    return View(model);

                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTypeSection(int? id)
        {
            if(id != null)
            {
                TypeSection typeSection = await db.TypeSections.FirstOrDefaultAsync(u => u.Id == id);
                if(typeSection != null)
                {
                     db.TypeSections.Remove(typeSection);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTypeSection(EditSectionViewModel model)
        {
            TypeSection typeSection = await db.TypeSections.FirstOrDefaultAsync(u => u.Name == model.Name);
            
            if (typeSection == null)
            {
                TypeSection _typeSection = new TypeSection()
                {
                    Name=model.Name
                };
                db.TypeSections.Add(_typeSection);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        
        [HttpGet]
        [ActionName("DeleteSection")]
        public async Task<IActionResult> ConfirmDeleteSection(int? id)
        {
            if(id != null)
            {
                Section Section = await db.Sections.FirstOrDefaultAsync(u => u.Id == id);
                if(Section != null)
                {
                    SectionViewModel model = new SectionViewModel()
                    {
                        Id = (int)id,
                        Name = Section.Name,
                        YearEvents = await db.YearEvents.Where(ye => ye.Section == Section.Name).ToListAsync(),
                        SubSections = await db.SubSections.Where(sub => sub.SectionId == Section.Id).ToListAsync()
                    };
                    return View(model);

                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSection(int? id)
        {
            if(id != null)
            {
                Section Section = await db.Sections.FirstOrDefaultAsync(u => u.Id == id);
                if(Section != null)
                {
                     db.Sections.Remove(Section);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSection(EditSectionViewModel model)
        {
            Section Section = await db.Sections.FirstOrDefaultAsync(u => u.Name == model.Name);
            
            if (Section == null)
            {
                Section _Section = new Section()
                {
                    Name=model.Name
                };
                db.Sections.Add(_Section);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        [HttpGet]
        [ActionName("DeleteDataYear")]
        public async Task<IActionResult> ConfirmDeleteDataYear(int? id)
        {
            if(id != null)
            {
                DataYear dataYear = await db.DataYears.FirstOrDefaultAsync(u => u.Id == id);
                if(dataYear != null)
                {
                    DataYearViewModel model = new DataYearViewModel()
                    {
                        Id = (int)id,
                        Name = dataYear.Name,
                        YearEvents = await db.YearEvents.Where(ye => ye.DataYear == dataYear.Name).ToListAsync()
                    };
                    return View(model);

                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDataYear(int? id)
        {
            if(id != null)
            {
                DataYear dataYear = await db.DataYears.FirstOrDefaultAsync(u => u.Id == id);
                if(dataYear != null)
                {
                     db.DataYears.Remove(dataYear);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDataYear(EditSectionViewModel model) 
        {
            DataYear dataYear = await db.DataYears.FirstOrDefaultAsync(u => u.Name == model.Name);
            
            if (dataYear == null)
            {
                DataYear _dataYear = new DataYear()
                {
                    Name=model.Name
                };
                db.DataYears.Add(_dataYear);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubSection(EditSectionViewModel model)
        {
            SubSection subSection = await db.SubSections.FirstOrDefaultAsync(sub => sub.Name == model.Name);
            if(subSection == null)
            {
                SubSection _subSection = new SubSection() 
                { 
                    Name = model.Name,
                    SectionId = model.id
                };
                db.SubSections.Add(_subSection);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> CreateSubSection1(EditSectionViewModel model)
        {
            SubSection1 subSection1 = await db.SubSection1s.FirstOrDefaultAsync(sub => sub.Name == model.Name);
            if(subSection1 == null)
            {
                SubSection1 _subSection1 = new SubSection1() 
                { 
                    Name = model.Name,
                    SubSectionId = model.id
                };
                db.SubSection1s.Add(_subSection1);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpGet]
        [ActionName("DeleteSubSection")]
        public async Task<IActionResult> ConfirmDeleteSubSection(int? id)
        {
            if (id != null)
            {
                SubSection subSection = await db.SubSections.FirstOrDefaultAsync(u => u.Id == id);
                if (subSection != null)
                {
                    SubSectionViewModel model = new SubSectionViewModel()
                    {
                        Id = (int)id,
                        Name = subSection.Name,
                        YearEvents = await db.YearEvents.Where(ye => ye.SubSection == subSection.Name).ToListAsync(),
                        SubSection1s = await db.SubSection1s.Where(sub => sub.SubSectionId == subSection.Id).ToListAsync()
                    };
                    return View(model);

                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSubSection(int? id)
        {
            if (id != null)
            {
                SubSection subSection = await db.SubSections.FirstOrDefaultAsync(u => u.Id == id);
                if (subSection != null)
                {
                    db.SubSections.Remove(subSection);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
        
        [HttpGet]
        [ActionName("DeleteSubSection1")]
        public async Task<IActionResult> ConfirmDeleteSubSection1(int? id)
        {
            if (id != null)
            {
                SubSection1 subSection1 = await db.SubSection1s.FirstOrDefaultAsync(u => u.Id == id);
                if (subSection1 != null)
                {
                    SubSection1ViewModel model = new SubSection1ViewModel()
                    {
                        Id = (int)id,
                        Name = subSection1.Name,
                        YearEvents = await db.YearEvents.Where(ye => ye.SubSection1 == subSection1.Name).ToListAsync()
                    };
                    return View(model);

                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSubSection1(int? id)
        {
            if (id != null)
            {
                SubSection1 subSection1 = await db.SubSection1s.FirstOrDefaultAsync(u => u.Id == id);
                if (subSection1 != null)
                {
                    db.SubSection1s.Remove(subSection1);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}