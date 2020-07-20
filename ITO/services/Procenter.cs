using ITO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.services
{
    public class Procenter
    {
        private readonly AllContext db;
       
        /// <summary>
        /// Возвращает процент выполнения годового плана по id пункта годового плана
        /// </summary>
        /// <param name="idYearEvent">id годового плана</param>
        /// <returns></returns>
        public async Task<decimal> GetProcentYearEvent(int idYearEvent, AllContext db)
        {
            YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(y => y.Id == idYearEvent);
            List<PartYearEvent> partYearEvents = await db.PartYearEvents
                .Where(p => p.YearEventId == yearEvent.Id && p.Сomment == null && p.UserNameСonfirmed != null).ToListAsync();

            decimal fullDonePlan = yearEvent.FirstQuarter + yearEvent.SecondQuarter + yearEvent.ThirdQuarter + yearEvent.FourthQuarter;
            decimal fullDoneNaw = 0;
            foreach (var part in partYearEvents)
            {
                fullDoneNaw += part.Done;
            }
            decimal procent = fullDoneNaw / fullDonePlan;
            return procent;
        }

        public async Task<decimal> GetProcentAgency(int idAgency, AllContext db, string dataYear)
        {

            Agency agency = await db.Agencies.FirstOrDefaultAsync(a => a.Id == idAgency);
            List<YearEvent> yearEvents = await db.YearEvents.Where(y => y.AgencyId == idAgency).
                Where(y => y.DataYear ==dataYear)
                .ToListAsync();
            List<decimal> procents = new List<decimal>();
            foreach(var yearEvent in yearEvents)
            {
                procents.Add(await GetProcentYearEvent(yearEvent.Id, db));
            }
            decimal aver = procents.Average();
            return aver;
        }

        public async Task<decimal> GetProcentTotal(List<Agency> agencies, AllContext db, string dataYear)
        {
            List<decimal> procents = new List<decimal>();
            foreach(Agency ag in agencies)
            {
                procents.Add(await GetProcentAgency(ag.Id, db, dataYear));
            }
            decimal aver = procents.Average();
            return aver;
        }
    }
}
