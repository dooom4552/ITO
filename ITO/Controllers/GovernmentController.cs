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
using ITO.services;

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
        public async Task<IActionResult> Index(string userName, string dataYear)
        {
            if (userName == User.Identity.Name)
            {
                if (User.Identity.Name != null)
                {
                    List<YearEvent> yearEvents = await db.YearEvents.Where(y => y.DataYear == dataYear).ToListAsync();
                    if (yearEvents.Count == 0)
                    {
                        dataYear = DateTime.Now.Year.ToString();
                        yearEvents = await db.YearEvents.Where(y => y.DataYear == dataYear).ToListAsync();
                        if (yearEvents.Count == 0)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }

                    List<Agency> agencies = await db.Agencies.ToListAsync();
                    agencies = AgencyFilter.GetAgenciesFilterYearEvent(agencies, yearEvents);

                    var agenciessort = from a in agencies
                                       orderby a.Name
                                       select a;// сортировка по имени учреждения
                    agencies = agenciessort.ToList();

                    if (agencies != null)
                    {
                        List<YearEventViewModel> YearEventViewModels = new List<YearEventViewModel>();

                        Procenter procenter = new Procenter();
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
                                PartYearEvents = await db.PartYearEvents.Where(p => p.YearEventId == yearEvent.Id).ToListAsync(),
                                Procent = await procenter.GetProcentYearEvent(yearEvent.Id, db),
                                TrClass = await Overdue.GetOverdueYearEvent(yearEvent.Id,db)
                            });
                        }
                        Pricer pricer = new Pricer();
                        Doner doner = new Doner();
                        List<AgencyYearPlanViewModel> agencyYearPlanViewModels = new List<AgencyYearPlanViewModel>();
                        foreach (Agency ag in agencies)
                        {

                            agencyYearPlanViewModels.Add(new AgencyYearPlanViewModel()
                            {
                                Id = ag.Id,
                                Name = ag.Name,
                                YearEventViewModels = YearEventViewModels.Where(y => y.AgencyId == ag.Id).ToList(),
                                FullDonePlan = await db.YearEvents.Where(y => y.AgencyId == ag.Id).
                                Where(y => y.DataYear == dataYear).
                                CountAsync(),
                                NowDonePlan = await doner.GetYearAgencyDoneNow(ag, db, dataYear),
                                Procent = await procenter.GetProcentAgency(ag.Id, db, dataYear),
                                FullPriceBnow = await pricer.GetFullPriceBNowAgency(ag.Id, db, dataYear),
                                FullPriceNotBnow = await pricer.GetFullPriceNotBNowAgency(ag.Id, db, dataYear)
                            });

                        }

                        TotalYearPlanViewModel totalYearPlanViewModel = new TotalYearPlanViewModel();
                        totalYearPlanViewModel.AgencyYearPlanViewModels = agencyYearPlanViewModels;
                        totalYearPlanViewModel.Procent = await procenter.GetProcentTotal(agencies, db, dataYear);
                        totalYearPlanViewModel.FullDonePlan = await doner.GetYearPlanCount(agencies, db, dataYear);
                        totalYearPlanViewModel.NowDonePlan = await doner.GetYearPlanDoneCount(agencies, db, dataYear);
                        totalYearPlanViewModel.FullPriceBnow = await pricer.GetFullPriceBNowTotal(agencies, db, dataYear);
                        totalYearPlanViewModel.FullPriceNotBnow = await pricer.GetFullPriceNotBNowTotal(agencies, db, dataYear);
                        totalYearPlanViewModel.FullPrice = totalYearPlanViewModel.FullPriceBnow + totalYearPlanViewModel.FullPriceNotBnow;
                        totalYearPlanViewModel.DataYear = dataYear;
                        totalYearPlanViewModel.DataYears = await db.DataYears.ToListAsync();
                        return View(totalYearPlanViewModel);

                    }

                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(int? Id)
        {
            if (Id != null)
            {
                PartYearEvent partYearEvent = await db.PartYearEvents.FirstOrDefaultAsync(p => p.Id == Id);
                if (partYearEvent != null)
                {
                    PartYearEventDetailsViewModel partYearEventDetailsViewModel = new PartYearEventDetailsViewModel()
                    {
                        Id = (int)Id,                        
                        NumberYearEvent = partYearEvent.NumberYearEvent,
                        Done = partYearEvent.Done,
                        Img = partYearEvent.Img,
                        Pdf = partYearEvent.Pdf,
                        PriceB = partYearEvent.PriceB,
                        PriceNotB = partYearEvent.PriceNotB,
                        DateTime = partYearEvent.DateTime,
                        UserNameСonfirmed = partYearEvent.UserNameСonfirmed,
                        UserNameSent = partYearEvent.UserNameSent
                    };
                    YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(ye => ye.Id == partYearEvent.YearEventId);

                    partYearEventDetailsViewModel.DataYear = yearEvent.DataYear;
                    partYearEventDetailsViewModel.EventText = yearEvent.EventText;
                    Agency agency = await db.Agencies.FirstOrDefaultAsync(a => a.Id == yearEvent.AgencyId);
                    partYearEventDetailsViewModel.Agency = agency.Name;
                    partYearEventDetailsViewModel.FullDonePlan = yearEvent.FirstQuarter + yearEvent.SecondQuarter + yearEvent.ThirdQuarter + yearEvent.FirstQuarter;
                    return View(partYearEventDetailsViewModel);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(PartYearEventDetailsViewModel partYearEventDetailsViewModel)
        {
            if (partYearEventDetailsViewModel != null)
            {
                PartYearEvent partYearEvent = await db.PartYearEvents.FirstOrDefaultAsync(p => p.Id == partYearEventDetailsViewModel.Id);
                partYearEvent.UserNameСonfirmed = User.Identity.Name;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { userName = User.Identity.Name });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> NotConfirm(PartYearEventDetailsViewModel partYearEventDetailsViewModel)
        {
            if (partYearEventDetailsViewModel != null)
            {
                PartYearEvent partYearEvent = await db.PartYearEvents.FirstOrDefaultAsync(p => p.Id == partYearEventDetailsViewModel.Id);
                partYearEvent.UserNameСonfirmed = User.Identity.Name;
                partYearEvent.Сomment = partYearEventDetailsViewModel.Сomment;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { userName = User.Identity.Name });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}