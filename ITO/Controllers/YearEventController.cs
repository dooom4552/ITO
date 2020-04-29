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
        private AllContext db;
        public YearEventController(AllContext context)
        {
            db = context;
        }
        public async Task <IActionResult> Index(int? id)
        {
            return View(await db.YearEvents.Where(ev => ev.Id == id).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(YearEvent yearEvent)
        {
            YearEvent _yearEvent = await db.YearEvents.FirstOrDefaultAsync((y => y.EventText == yearEvent.EventText | y.Number==yearEvent.Number ));
            if (_yearEvent == null)
            {
                int number = db.YearEvents.Where(ev => ev.Agency == yearEvent.Agency).ToList().Count+1;
                yearEvent.Number = number;
                db.YearEvents.Add(yearEvent);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

    }
}