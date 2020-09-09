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
        public static async Task<string> GetOverdueYearEventColor(int Id, AllContext db)
        {
            switch (await GetOverdueYearEvent(Id, db))
            {
                case 1:
                    return "bg-info";
                case 2:
                    return "bg-warning";
                case 3:
                    return "bg-success";
                case 4:
                    return "bg-danger";
                default:
                    return "bg-info";
            }                          
        }
        public static  string GetOverdueYearEventColorNotAsync(int Id, List<PartYearEvent> partYearEvents, int FirstQuarter, int SecondQuarter, int ThirdQuarter, int FourthQuarter)
        {
            switch (GetOverdueYearEventNotAsync(Id, partYearEvents,  FirstQuarter,  SecondQuarter,  ThirdQuarter,  FourthQuarter))
            {
                case 1:
                    return "bg-info";
                case 2:
                    return "bg-warning";
                case 3:
                    return "bg-success";
                case 4:
                    return "bg-danger";
                default:
                    return "bg-info";
            }                          
        }








        private static async Task<int> GetOverdueYearEvent(int Id, AllContext db)
        {
            YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(y => y.Id == Id);
            List<PartYearEvent> AgreedPartYearEvents = await db.PartYearEvents
                .Where(p => p.YearEventId == yearEvent.Id)
                .Where(p => p.UserNameSent != null)
                .Where(p => p.UserNameСonfirmed == null)
                .Where(p => p.Сomment == null)
                .ToListAsync();

            List<PartYearEvent> ReturnPartYearEvents = await db.PartYearEvents
               .Where(p => p.YearEventId == yearEvent.Id)
               .Where(p => p.UserNameSent != null)
               .Where(p => p.Сomment != null)
               .ToListAsync();


            //на согласовании
            if (AgreedPartYearEvents.Count > 0)
            {
                return 1;//"bg-info";
            }
            //возвращен на доработку"
            else if (ReturnPartYearEvents.Count > 0)
            {
                return 2;// "bg-warning";
            }

            //сколько запланировано на сегодня
            int planQuarterNow = 0;

            //сколько выполнено на сегодня
            int fullDoneNaw = await Doner.GetNowDone(yearEvent.Id, db);

            if (DateTime.Now.DayOfYear <= 90)//1 квартал
            {
                planQuarterNow = yearEvent.FirstQuarter;
            }
            else if (90 < DateTime.Now.DayOfYear && DateTime.Now.DayOfYear <= 181)//2 квартал
            {
                planQuarterNow = yearEvent.FirstQuarter
                    + yearEvent.SecondQuarter;
            }
            else if (181 < DateTime.Now.DayOfYear && DateTime.Now.DayOfYear <= 272)//3 квартал
            {
                planQuarterNow = yearEvent.FirstQuarter
                    + yearEvent.SecondQuarter
                    + yearEvent.ThirdQuarter;
            }
            else if (272 < DateTime.Now.DayOfYear)//4 квартал
            {
                planQuarterNow = yearEvent.FirstQuarter
                    + yearEvent.SecondQuarter
                    + yearEvent.ThirdQuarter
                    + yearEvent.FourthQuarter;
            }

            //пункт выполнен
            if (fullDoneNaw >= planQuarterNow)
            {
                return 3;// "bg-success";
            }
            //просрочен
            else
            {
                return 4;// "bg-danger";
            }
        }

        /// <summary>
        /// возвращает число int от 1 до 5 соотвествует цветам класса
        /// </summary>
        /// <param name="Id">ID годового плана</param>
        /// <param name="db"></param>
        /// <returns></returns>
        private static  int GetOverdueYearEventNotAsync(int Id, List<PartYearEvent> partYearEvents, int FirstQuarter, int SecondQuarter, int ThirdQuarter, int FourthQuarter)
        {
            List<PartYearEvent> AllpartYearEvents = partYearEvents
                .Where(p => p.YearEventId == Id)
                .Where(p => p.UserNameSent != null).ToList();

            ///отчеты на согласовании
            bool AgreedPart = AllpartYearEvents
                .Where(p => p.UserNameСonfirmed == null)
                .Where(p => p.Сomment == null)
                .Any();
            ///отчеты возвращенные на доработку
           bool ReturnPart = AllpartYearEvents
               .Where(p => p.Сomment != null)
               .Any();


            //на согласовании
            if (AgreedPart)
            {
                return 1;//"bg-info";
            }
            //возвращен на доработку"
            else if (ReturnPart)
            {
                return 2;// "bg-warning";
            }

            //сколько запланировано на сегодня
            int planQuarterNow = 0;

            //сколько выполнено на сегодня
            int fullDoneNaw = AllpartYearEvents
                .Where(p => p.UserNameСonfirmed != null) //ктото подтвердил
                .Where(p => p.Сomment == null) // не возвращен на доработку
                .Sum(p => p.Done); //Doner.GetNowDoneNotAsync(Id, db);

            if (DateTime.Now.DayOfYear <= 90)//1 квартал
            {
                planQuarterNow = FirstQuarter;
            }
            else if (90 < DateTime.Now.DayOfYear && DateTime.Now.DayOfYear <= 181)//2 квартал
            {
                planQuarterNow = FirstQuarter + SecondQuarter;
            }
            else if (181 < DateTime.Now.DayOfYear && DateTime.Now.DayOfYear <= 272)//3 квартал
            {
                planQuarterNow = FirstQuarter + SecondQuarter + ThirdQuarter;
            }
            else if (272 < DateTime.Now.DayOfYear)//4 квартал
            {
                planQuarterNow = FirstQuarter + SecondQuarter + ThirdQuarter + FourthQuarter;
            }

            //пункт выполнен
            if (fullDoneNaw >= planQuarterNow)
            {
                return 3;// "bg-success";
            }
            //просрочен
            else
            {
                return 4;// "bg-danger";
            }
        }


    }
}
