using ITO.Models;
using ITO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.Controllers
{
    [Authorize(Roles = "admin")]
    public class AgencyController : Controller
    {        
        private readonly AllContext db;
        RoleManager<IdentityRole> _roleManager;

        public AgencyController(AllContext context, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            db = context;
        }
        public async Task <IActionResult> Index()
        {
            return View(await db.Agencies.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Agency agency)
        {
            Agency _agency= await db.Agencies.FirstOrDefaultAsync(a => a.Name == agency.Name);
            if(_agency==null)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(agency.Name));
                db.Agencies.Add(agency);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
       
        public async Task<IActionResult> Details(int? id, string DataYearVM)
        {
            if(id != null)
            {

                if (DataYearVM == null)
                {
                    DataYearVM = DateTime.Now.Year.ToString();
                }

                DetailsAgencyViewModel model = new DetailsAgencyViewModel()
                {
                    AgencyId = (int)id,
                    Agency = await db.Agencies.FirstOrDefaultAsync(a => a.Id == id),
                    YearEvents =await db.YearEvents.Where(ye => ye.AgencyId == id && ye.DataYear== DataYearVM).ToListAsync(),
                    DataYears =  await db.DataYears.ToListAsync(),               
                };
                if (model != null)
                    return View(model);
            }
            return NotFound();
        }





        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if(id != null)
            {
                Agency agency = await db.Agencies.FirstOrDefaultAsync(a => a.Id == id);
                if (agency != null)
                {
                    DetailsAgencyViewModel model = new DetailsAgencyViewModel()
                    {
                        Agency = await db.Agencies.FirstOrDefaultAsync(ag => ag.Id == id),
                        AgencyId = (int)id,
                        YearEvents = await db.YearEvents.Where(ye=> ye.AgencyId == agency.Id).ToListAsync(),                       
                    };
                    return View(model);
                }                    
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            Agency agency = await db.Agencies.FirstOrDefaultAsync(a => a.Id == id);
            if (agency != null)
            {
                db.Agencies.Remove(agency);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
