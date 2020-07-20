using ITO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.services
{
    public class Doner
    {
        /// <summary>
        /// Возвращает количество необходимое для полного завершения данного пункта годового плана по id отчета
        /// </summary>
        /// <param name="idPartYearEvent">id отчета годового плана</param>
        /// <returns></returns>
        public async Task<int> GetMaxDone(int? idPartYearEvent, AllContext db)
        {
            if (idPartYearEvent != null)
            {
                PartYearEvent partYearEvent = await db.PartYearEvents.FirstOrDefaultAsync(p => p.Id == idPartYearEvent);
                YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(y => y.Id == partYearEvent.YearEventId);
                int fullDonePlan = yearEvent.FirstQuarter + yearEvent.SecondQuarter + yearEvent.ThirdQuarter + yearEvent.FirstQuarter;

                List<PartYearEvent> partYearEventsСonfirmed = await db.PartYearEvents
                    .Where(p => p.YearEventId == partYearEvent.YearEventId && p.Сomment == null && p.UserNameСonfirmed != null).ToListAsync();
                int fullDoneNaw = 0;
                foreach (var part in partYearEventsСonfirmed)
                {
                    fullDoneNaw += part.Done;
                }
                return fullDonePlan - fullDoneNaw;
            }
            return 0;
        }
        /// <summary>
        /// сколько запланировано
        /// </summary>
        /// <param name="agencies"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<int> GetYearPlanCount(List<Agency> agencies, AllContext db, string dataYear)
        {
            int number = 0;
            foreach (Agency agency in agencies)
            {
                number += await db.YearEvents.Where(y => y.AgencyId == agency.Id)
                    .Where( y => y.DataYear == dataYear)
                    .CountAsync();
            }
            return number;
        }
        /// <summary>
        /// сколько выполнено
        /// </summary>
        /// <param name="agencies"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<int> GetYearPlanDoneCount(List<Agency> agencies, AllContext db, string dataYear)
        {
            Procenter procenter = new Procenter();
            List<YearEvent> yearEvents = new List<YearEvent>();
            List<YearEvent> yearEventsFilter = new List<YearEvent>();
            int number = 0;

            foreach (Agency agency in agencies)
            {
                yearEvents.AddRange(db.YearEvents
                    .Where(y => y.AgencyId == agency.Id)
                    .Where(y => y.DataYear == dataYear));
            }
            foreach (YearEvent yearEvent in yearEvents)
            {
                if (await procenter.GetProcentYearEvent(yearEvent.Id, db) == 1)
                {
                    yearEventsFilter.Add(yearEvent);
                }
            }
            number = yearEventsFilter.Count();
            return number;
        }
        public async Task<int> GetYearAgencyDoneNow(Agency agency, AllContext db, string dataYear)
        {
            Procenter procenter = new Procenter();
            List<YearEvent> yearEvents = new List<YearEvent>();
            List<YearEvent> yearEventsFilter = new List<YearEvent>();
            yearEvents.AddRange(db.YearEvents.Where(y => y.AgencyId == agency.Id)
                                             .Where(y => y.DataYear == dataYear));

            foreach (YearEvent yearEvent in yearEvents)
            {
                if (await procenter.GetProcentYearEvent(yearEvent.Id, db) == 1)
                {
                    yearEventsFilter.Add(yearEvent);
                }
            }
            int number = yearEventsFilter.Count();
            return number;
        }
        /// <summary>
        /// возвращает сколько выполнено по данному пункту на сегодня
        /// </summary>
        /// <param name="YeId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static async Task<int> GetNowDone(int YeId, AllContext db)
        {
            List<PartYearEvent> partYearEventsСonfirmed = await db.PartYearEvents
                    .Where(p => p.YearEventId == YeId && p.Сomment == null && p.UserNameСonfirmed != null).ToListAsync();
            int fullDoneNaw = 0;
            foreach (var part in partYearEventsСonfirmed)
            {
                fullDoneNaw += part.Done;
            }
            return fullDoneNaw;
        }
    }
}
