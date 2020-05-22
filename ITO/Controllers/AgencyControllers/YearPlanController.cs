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
        public async Task <IActionResult> Index(User userAgency)
        {
            if (userAgency != null)
            {
                var userAgencyRoles = await _userManager.GetRolesAsync(userAgency);
                if (userAgencyRoles.Count > 0)
                {
                    foreach (Agency ag in db.Agencies)
                    {
                        if (userAgencyRoles.FirstOrDefault(rol => rol == ag.Name) != null)
                        {
                            List<YearEvent> yearEvents = await db.YearEvents.Where(ye => ye.AgencyId == ag.Id).ToListAsync();
                            return View(yearEvents);
                        }
                    }
                }
            }
            //return NotFound();
            return View();
        }
    }
}