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
using ITO.services;

namespace ITO.Controllers.AgencyControllers
{
    [Authorize(Roles = "учреждение, управление")]
    public class YearPlanController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        readonly UserManager<User> _userManager;
        private readonly AllContext db;
        IWebHostEnvironment _appEnvironment;

        public YearPlanController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, AllContext context, IWebHostEnvironment appEnvironment)
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
                Agency ag = new Agency();
                if (User.Identity.Name != null)
                {
                    User userAgency = await _userManager.FindByNameAsync(User.Identity.Name);
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
                            if (ag.Name != null)
                            {
                                List<YearEvent> yearEvents = await db.YearEvents.Where(ye => ye.AgencyId == ag.Id)
                                    .Where(ye => ye.DataYear == dataYear)
                                    .ToListAsync();
                                List<YearEventViewModel> yearEventsViewModel = new List<YearEventViewModel>();
                                Procenter procenter = new Procenter();
                                Pricer pricer = new Pricer();
                                AgencyYearPlanViewModel agencyYearPlanViewModel = new AgencyYearPlanViewModel();

                                foreach (YearEvent yearEvent in yearEvents)
                                {
                                    yearEventsViewModel.Add(new YearEventViewModel()
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
                                        FullPriceBnow = await pricer.GetFullPriceBNow(yearEvent.Id, db),
                                        FullPriceNotBnow = await pricer.GetFullPriceNotBNow(yearEvent.Id, db),                                        
                                        NumberPartReturnsandSent = await GetNumberPartReturnsAndSent(yearEvent.Id),
                                        TrClass =await Overdue.GetOverdueYearEventColor(yearEvent.Id,db)
                                    });
                                }
                                agencyYearPlanViewModel.YearEventViewModels = yearEventsViewModel;
                                agencyYearPlanViewModel.DataYears = await DataYearFilter.GetDataYear(ag.Id, db);
                                agencyYearPlanViewModel.DataYear = dataYear;
                                agencyYearPlanViewModel.Name = ag.Name;
                                return View(agencyYearPlanViewModel);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Execute(int YearEventId)
        {
            YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(ye => ye.Id == YearEventId);
            if (yearEvent != null)
            {
                int totalDone = 0;
                int maxDone = 0;
                List<PartYearEvent> partYearEvents = await db.PartYearEvents.Where(p => p.YearEventId == YearEventId).ToListAsync();

                int numberPartReturns = partYearEvents.Where(p => p.UserNameСonfirmed != null && p.Сomment != null).Count();// количество отчетов которые вернули на доработку
                int numberPartSent = partYearEvents.Where(p => p.UserNameСonfirmed == null && p.UserNameSent != null).Count();// количество отчетов еще на рассмотрении

                if ((numberPartReturns + numberPartSent) == 0)
                {

                    if (partYearEvents != null)
                    {
                        foreach (PartYearEvent part in partYearEvents)
                        {
                            totalDone += part.Done;
                        }
                        maxDone = yearEvent.FirstQuarter + yearEvent.SecondQuarter + yearEvent.ThirdQuarter + yearEvent.FourthQuarter - totalDone;
                    }
                    PartYearEventViewModel partYearEventViewModel = new PartYearEventViewModel()
                    {
                        YearEventId = yearEvent.Id,
                        NumberYearEvent = yearEvent.Number,
                        EventText = yearEvent.EventText,
                        FirstQuarter = yearEvent.FirstQuarter,
                        SecondQuarter = yearEvent.SecondQuarter,
                        ThirdQuarter = yearEvent.ThirdQuarter,
                        FourthQuarter = yearEvent.FourthQuarter,
                        Unit = yearEvent.Unit,
                        DataYear = yearEvent.DataYear,
                        maxDone = maxDone
                    };
                    return View(partYearEventViewModel);
                }

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Execute(PartYearEventViewModel partYearEventViewModel)
        {
            if (partYearEventViewModel != null)
            {
                YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(y => y.Id == partYearEventViewModel.YearEventId);
                Agency agency = await db.Agencies.FirstOrDefaultAsync(ag => ag.Id == yearEvent.AgencyId);

                // путь к папке Files
                string path = Path.Combine(_appEnvironment.WebRootPath, "img");
                //string pathWWWRootNot = "\\img\\";

                if (!Directory.Exists(Path.Combine(path, agency.Name)))
                {
                    Directory.CreateDirectory(Path.Combine(path, agency.Name));
                }

                path = Path.Combine(path, agency.Name);
                string pathWWWRootNot = Path.Combine("img", agency.Name);

                if (!Directory.Exists(Path.Combine(path, partYearEventViewModel.DataYear)))
                {
                    Directory.CreateDirectory(Path.Combine(path, partYearEventViewModel.DataYear));
                }
                path = Path.Combine(path, partYearEventViewModel.DataYear);
                pathWWWRootNot = Path.Combine(pathWWWRootNot, partYearEventViewModel.DataYear);

                if (!Directory.Exists(Path.Combine(path, partYearEventViewModel.NumberYearEvent.ToString())))
                {
                    Directory.CreateDirectory(Path.Combine(path, partYearEventViewModel.NumberYearEvent.ToString()));
                }

                path = Path.Combine(path, partYearEventViewModel.NumberYearEvent.ToString());
                pathWWWRootNot = Path.Combine(pathWWWRootNot, partYearEventViewModel.NumberYearEvent.ToString());

                string fullFileNameImageFile;
                string fullFileNamePdfFile;

                string extensionImageFile = Path.GetExtension(partYearEventViewModel.ImageFile.FileName);
                string extensionPdfFile = Path.GetExtension(partYearEventViewModel.PdfFile.FileName);

                string fileNameImageFile = $"ГП_image_{agency.Name}_{partYearEventViewModel.NumberYearEvent}_{DateTime.Now:dd-MM-yyyy-hh-mm-ss}{extensionImageFile}";
                string fileNamePdfFile = $"ГП_pdf_{agency.Name}_{partYearEventViewModel.NumberYearEvent}_{DateTime.Now:dd-MM-yyyy-hh-mm-ss}{extensionPdfFile}";

                using (var fileStream = new FileStream(Path.Combine(path, fileNameImageFile), FileMode.Create))
                {
                    await partYearEventViewModel.ImageFile.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                    fullFileNameImageFile = "\\" + Path.Combine(pathWWWRootNot, fileNameImageFile);
                }

                using (var fileStream = new FileStream(Path.Combine(path, fileNamePdfFile), FileMode.Create))
                {
                    await partYearEventViewModel.PdfFile.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                    fullFileNamePdfFile = "\\" + Path.Combine(pathWWWRootNot, fileNamePdfFile);
                }

                PartYearEvent partYearEvent = new PartYearEvent()
                {
                    YearEventId = partYearEventViewModel.YearEventId,
                    NumberYearEvent = partYearEventViewModel.NumberYearEvent,
                    Done = partYearEventViewModel.Done,
                    PriceB = partYearEventViewModel.PriceB,
                    PriceNotB = partYearEventViewModel.PriceNotB,
                    DateTime = DateTime.Now,
                    UserNameSent = User.Identity.Name,
                    Img = fullFileNameImageFile,
                    Pdf = fullFileNamePdfFile
                };
                await db.PartYearEvents.AddAsync(partYearEvent);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index", new { userName = User.Identity.Name, dataYear = partYearEventViewModel.DataYear });
        }


        [HttpPost]
        public async Task<IActionResult> PartDetails(PartYearEvent _partYearEvent)
        {
            PartYearEvent partYearEvent = await db.PartYearEvents.FirstOrDefaultAsync(p => p.Id == _partYearEvent.Id);
            if (partYearEvent != null)
            {
                PartYearEventDetailsViewModel partYearEventDetailsViewModel = new PartYearEventDetailsViewModel()
                {
                    NumberYearEvent = partYearEvent.NumberYearEvent,
                    Done = partYearEvent.Done,
                    Img = partYearEvent.Img,
                    Pdf = partYearEvent.Pdf,
                    PriceB = partYearEvent.PriceB,
                    PriceNotB = partYearEvent.PriceNotB,
                    DateTime = partYearEvent.DateTime,
                    UserNameСonfirmed = partYearEvent.UserNameСonfirmed,
                    UserNameSent = partYearEvent.UserNameSent,
                    Сomment = partYearEvent.Сomment
                };
                YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(ye => ye.Id == partYearEvent.YearEventId);

                partYearEventDetailsViewModel.DataYear = yearEvent.DataYear;
                partYearEventDetailsViewModel.EventText = yearEvent.EventText;
                Agency agency = await db.Agencies.FirstOrDefaultAsync(a => a.Id == yearEvent.AgencyId);
                partYearEventDetailsViewModel.Agency = agency.Name;
                partYearEventDetailsViewModel.FullDonePlan = yearEvent.FirstQuarter + yearEvent.SecondQuarter + yearEvent.ThirdQuarter + yearEvent.FirstQuarter;
                return View(partYearEventDetailsViewModel);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Return(int? Id)
        {
            if (Id != null)
            {
                PartYearEvent partYearEvent = await db.PartYearEvents.FirstOrDefaultAsync(p => p.Id == Id);
                if (partYearEvent != null)
                {
                    if (await Success(partYearEvent))
                    {
                        Doner doner = new Doner();
                        PartYearEventViewModel partYearEventViewModel = new PartYearEventViewModel
                        {
                            Id = partYearEvent.Id,
                            YearEventId = partYearEvent.YearEventId,
                            NumberYearEvent = partYearEvent.NumberYearEvent,
                            Done = partYearEvent.Done,
                            Img = partYearEvent.Img,
                            Pdf = partYearEvent.Pdf,
                            PriceB = partYearEvent.PriceB,
                            PriceNotB = partYearEvent.PriceNotB,
                            DateTime = partYearEvent.DateTime,
                            UserNameСonfirmed = partYearEvent.UserNameСonfirmed,
                            UserNameSent = partYearEvent.UserNameSent,
                            Сomment = partYearEvent.Сomment,
                            maxDone = await doner.GetMaxDone(partYearEvent.Id, db)
                        };

                        return View(partYearEventViewModel);
                    }

                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Return(PartYearEventViewModel partYearEventViewModel)
        {
            if (partYearEventViewModel != null)
            {
                PartYearEvent partYearEvent = await db.PartYearEvents.FirstOrDefaultAsync(p => p.Id == partYearEventViewModel.Id);
                {
                    if (partYearEvent != null)
                    {
                        string path = _appEnvironment.WebRootPath;
                        if (partYearEventViewModel.Done != 0)
                        {
                            partYearEvent.Done = partYearEventViewModel.Done;
                        }
                        partYearEvent.PriceB = partYearEventViewModel.PriceB;
                        partYearEvent.PriceNotB = partYearEventViewModel.PriceNotB;

                        if (partYearEventViewModel.PdfFile != null)
                        {
                            using (var fileStream = new FileStream(path + partYearEvent.Pdf, FileMode.Create))
                            {
                                await partYearEventViewModel.PdfFile.CopyToAsync(fileStream);
                                await fileStream.FlushAsync();
                            }
                        }

                        if (partYearEventViewModel.ImageFile != null)
                        {
                            using (var fileStream = new FileStream(path + partYearEvent.Img, FileMode.Create))
                            {
                                await partYearEventViewModel.ImageFile.CopyToAsync(fileStream);
                                await fileStream.FlushAsync();
                            }
                        }
                        partYearEvent.Сomment = null;
                        partYearEvent.UserNameСonfirmed = null;
                        await db.SaveChangesAsync();
                        //string test =await DataYearFilter.GetStringDataYear(partYearEvent.YearEventId, db);
                        return RedirectToAction("Index", new { userName = User.Identity.Name, dataYear= await DataYearFilter.GetStringDataYear(partYearEvent.YearEventId, db) });
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PartYearEventViewModel partYearEventViewModel)
        {
            if (partYearEventViewModel != null)
            {
                PartYearEvent partYearEvent = await db.PartYearEvents.FirstOrDefaultAsync(p => p.Id == partYearEventViewModel.Id);
                if (partYearEvent != null)
                {
                    db.PartYearEvents.Remove(partYearEvent);
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Проверяет принадлежит ли данный отчет авторезированому в данный момент пользователю
        /// </summary>
        ///<param name="partYearEvent">отчет годового плана</param>
        /// <returns></returns>
        public async Task<bool> Success(PartYearEvent partYearEvent)
        {
            YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(y => y.Id == partYearEvent.YearEventId);//получаем год план

            Agency agency = await db.Agencies.FirstOrDefaultAsync(a => a.Id == yearEvent.AgencyId);// получаем учреждение

            User userAgency = await _userManager.FindByNameAsync(User.Identity.Name);//пользователь который авторизован

            if (userAgency != null)//если авторизован
            {
                var userAgencyRoles = await _userManager.GetRolesAsync(userAgency);//перечень ролей пользователя авторизованного в данный момент

                if (userAgencyRoles.Contains(agency.Name))//если среди ролей есть название учреждения
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Проверяет есть ли по данному пункту годового плана еще не рассмотренные отчеты и отправленные на доработку, проверка нужна для отоброжения кнопки ВЫПОЛНИТЬ, если есть отчеты отправленные на доработку или еще не рассмотренные то выполнить нельзя
        /// </summary>
        /// <param name="YearEventId">id пункта годового плана</param>
        /// <returns></returns>
        public async Task<bool> GetNumberPartReturnsAndSent(int? YearEventId)
        {
            if (YearEventId != null)
            {
                YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(ye => ye.Id == YearEventId);
                if (yearEvent != null)
                {
                    List<PartYearEvent> partYearEvents = await db.PartYearEvents.Where(p => p.YearEventId == YearEventId).ToListAsync();
                    int numberPartReturns = partYearEvents.Where(p => p.UserNameСonfirmed != null && p.Сomment != null).Count();// количество отчетов которые вернули на доработку
                    int numberPartSent = partYearEvents.Where(p => p.UserNameСonfirmed == null && p.UserNameSent != null).Count();// количество отчетов еще на рассмотрении

                    if (numberPartReturns + numberPartSent > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}