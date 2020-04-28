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
        private ApplicationContext db;
        public AgencyController(ApplicationContext context)
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
            db.Agencies.Add(agency);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
