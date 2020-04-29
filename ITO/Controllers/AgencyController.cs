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
    public class AgencyController : Controller
    {        
        private AllContext db;
        public AgencyController(AllContext context)
        {          
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
                db.Agencies.Add(agency);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                Agency agency = await db.Agencies.FirstOrDefaultAsync(a => a.Id == id);
                if (agency != null)
                    return View(agency);
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
                    return View(agency);
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
