using ITO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.services
{
    public class Overdue
    {
        /// <summary>
        /// возврашает цвет выпонен пункт или нет
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static async Task<string> GetOverdueYearEvent(int Id, AllContext db)
        {
            YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(y => y.Id == Id);

            //есть ли отчеты на согласовании
            //bool agreed = await db.PartYearEvents.Where(p => p.YearEventId ==yearEvent.Id)
            //    .AnyAsync(p => p.UserNameSent != null && p.UserNameСonfirmed == null && p.Сomment == null);
            //bool agreed = await db.PartYearEvents
            //    .Where(p => p.YearEventId == yearEvent.Id)
            //    .Where(p => p.UserNameSent != null)
            //    .Where(p => p.UserNameСonfirmed == null)
            //    .Where(p => p.Сomment == null)
            //    .AnyAsync();
            List<PartYearEvent> AgreedPartYearEvents = await db.PartYearEvents
                .Where(p => p.YearEventId == yearEvent.Id)
                .Where(p => p.UserNameSent != null)
                .Where(p => p.UserNameСonfirmed == null)
                .Where(p => p.Сomment == null)
                .ToListAsync();

            //есть ли возвращенные отчеты
            //bool returned = await db.PartYearEvents.Where(p => p.YearEventId == yearEvent.Id)
            //    .AnyAsync(p => p.UserNameSent != null && p.Сomment != null);
            //bool returned = await db.PartYearEvents
            //    .Where(p => p.YearEventId == yearEvent.Id)
            //    .Where(p => p.UserNameSent != null)
            //    .Where(p => p.Сomment != null).AnyAsync();

            List<PartYearEvent> ReturnPartYearEvents = await db.PartYearEvents
               .Where(p => p.YearEventId == yearEvent.Id)
               .Where(p => p.UserNameSent != null)              
               .Where(p => p.Сomment != null)
               .ToListAsync();


            //на согласовании
            if (AgreedPartYearEvents.Count > 0)
            {
                return "bg-info";
            }
            //возвращен на доработку"
            if (ReturnPartYearEvents.Count > 0)
            {
                return "bg-warning";
            }

            int planQuarterNow = 0;//сколько запланировано на сегодня
            int fullDoneNaw =await Doner.GetNowDone(yearEvent.Id, db);//сколько выполнено на сегодня
            if (DateTime.Now.DayOfYear <= 90)//1 квартал
            {
                planQuarterNow = yearEvent.FirstQuarter;                
            }
            else if(90 < DateTime.Now.DayOfYear && DateTime.Now.DayOfYear <= 181)//2 квартал
            {
                planQuarterNow = yearEvent.FirstQuarter
                    + yearEvent.SecondQuarter;
            }
            else if(181< DateTime.Now.DayOfYear && DateTime.Now.DayOfYear <= 272)//3 квартал
            {
                planQuarterNow = yearEvent.FirstQuarter
                    + yearEvent.SecondQuarter
                    + yearEvent.ThirdQuarter;
            }
            else if(272 < DateTime.Now.DayOfYear)//4 квартал
            {
                planQuarterNow = yearEvent.FirstQuarter
                    + yearEvent.SecondQuarter
                    + yearEvent.ThirdQuarter
                    + yearEvent.FourthQuarter;
            }

            //пункт выполнен
            if (fullDoneNaw== planQuarterNow)
            {
                return "bg-success";
            }
            //просрочен
            else
            {
                return "bg-danger";
            }
        }
    }
}
