using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITO.Models;
using ITO.ViewModels;
using ITO.ViewModels.Sections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITO.Controllers
{
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
    }
}