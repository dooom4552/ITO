using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ITO.Models;
using Microsoft.EntityFrameworkCore;

namespace ITO.Controllers.AgencyControllers
{
    public class YearPlanController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        readonly UserManager<User> _userManager;
        private readonly AllContext db;

        public YearPlanController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, AllContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            db = context;
        }

        [Authorize(Roles = "учреждение")] 
        [HttpGet]
        public async Task <IActionResult> Index(string userName)
        {
            Agency ag = new Agency();
            if (userName != null)
            {
                User userAgency = await _userManager.FindByNameAsync(userName);
                if (userAgency != null)
                {
                    var userAgencyRoles = await _userManager.GetRolesAsync(userAgency);
                    if (userAgencyRoles.Count > 0)
                    {
                        foreach (Agency agg in db.Agencies)
                        {
                            if (userAgencyRoles.FirstOrDefault(rol => rol == agg.Name) != null)
                            {
                                ag = agg;
                            }
                        }
                        List<YearEvent> yearEvents = await db.YearEvents.Where(ye => ye.AgencyId == ag.Id).ToListAsync();                        
                        int i = yearEvents.Count;
                        return View(yearEvents);
                    }
                }
            }
            return NotFound();

        }

        [HttpGet]
        public async Task <IActionResult> Execute(int YearEventId)
        {
            YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(ye => ye.Id == YearEventId);
            if(yearEvent != null)
            {

            }
            return RedirectToAction("Index", "Home");
        }
    }
}