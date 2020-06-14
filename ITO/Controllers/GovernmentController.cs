using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ITO.Models;
using Microsoft.EntityFrameworkCore;
using ITO.ViewModels.AgencyUser;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ITO.ViewModels.GovernmentUser;

namespace ITO.Controllers
{
    [Authorize(Roles = "управление")]
    public class GovernmentController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        readonly UserManager<User> _userManager;
        private readonly AllContext db;
        IWebHostEnvironment _appEnvironment;

        public GovernmentController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, AllContext context, IWebHostEnvironment appEnvironment)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            db = context;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string userName)
        {
            if (userName == User.Identity.Name)
            {
                if (User.Identity.Name != null)
                {
                    List<Agency> agencies = await db.Agencies.ToListAsync();
                    if (agencies != null)
                    {
                        List<YearEventViewModel> YearEventViewModels = new List<YearEventViewModel>();

                        List<YearEvent> yearEvents = await db.YearEvents.ToListAsync();
                        foreach (YearEvent yearEvent in yearEvents)
                        {
                            YearEventViewModels.Add(new YearEventViewModel()
                            {
                                Id = yearEvent.Id,
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
                                PartYearEvents = await db.PartYearEvents.Where(p => p.YearEventId == yearEvent.Id).ToListAsync()
                            });
                        }

                        List<ListAgencyViewModel> listAgencyViewModels = new List<ListAgencyViewModel>();
                        foreach (Agency ag in agencies)
                        {

                            listAgencyViewModels.Add(new ListAgencyViewModel()
                            {
                                Id = ag.Id,
                                Name = ag.Name,
                                YearEventViewModels = YearEventViewModels.Where(y => y.AgencyId == ag.Id).ToList(),
                                FullDonePlan = await db.YearEvents.Where(y => y.AgencyId == ag.Id).CountAsync(),
                            });

                        }
                        return View(listAgencyViewModels);

                    }
                }
            }
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(int? Id)
        {
            if (Id != null)
            {
                PartYearEvent partYearEvent = await db.PartYearEvents.FirstOrDefaultAsync(p => p.Id == Id);
                if (partYearEvent != null)
                {
                    return View(partYearEvent);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(PartYearEvent _partYearEvent)
        {
            if (_partYearEvent != null)
            {
                PartYearEvent partYearEvent = await db.PartYearEvents.FirstOrDefaultAsync(p => p.Id == _partYearEvent.Id);
                partYearEvent.UserNameСonfirmed = User.Identity.Name;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { userName = User.Identity.Name });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> NotConfirm(PartYearEvent _partYearEvent)
        {
            if (_partYearEvent != null)
            {
                PartYearEvent partYearEvent = await db.PartYearEvents.FirstOrDefaultAsync(p => p.Id == _partYearEvent.Id);
                partYearEvent.UserNameСonfirmed = User.Identity.Name;
                partYearEvent.Сomment = _partYearEvent.Сomment;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { userName = User.Identity.Name });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}