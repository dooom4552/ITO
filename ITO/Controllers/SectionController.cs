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
    }
}