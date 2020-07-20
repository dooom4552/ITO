using ITO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.services
{
    public class Pricer
    {
        /// <summary>
        /// Возвращает количество затраченных бюджетных средств на выполнения данного пункта годового плана
        /// </summary>
        /// <param name="idYearEvent">id пункта годового плана</param>
        /// <returns></returns>
        public async Task<float> GetFullPriceBNow(int idYearEvent, AllContext db)
        {
            YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(y => y.Id == idYearEvent);
            List<PartYearEvent> partYearEvents = await db.PartYearEvents
                .Where(p => p.YearEventId == yearEvent.Id && p.Сomment == null && p.UserNameСonfirmed != null).ToListAsync();

            float FullPriceBnow = 0;

            foreach (var part in partYearEvents)
            {
                FullPriceBnow += part.PriceB;
            }
            return FullPriceBnow;
        }

        /// <summary>
        /// Возвращает количество затраченных внебюджетных средств на выполнения данного пункта годового плана
        /// </summary>
        /// <param name="idYearEvent">id пункта годового плана</param>
        /// <returns></returns>
        public async Task<float> GetFullPriceNotBNow(int idYearEvent, AllContext db)
        {
            YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(y => y.Id == idYearEvent);
            List<PartYearEvent> partYearEvents = await db.PartYearEvents
                .Where(p => p.YearEventId == yearEvent.Id && p.Сomment == null && p.UserNameСonfirmed != null).ToListAsync();

            float FullPriceNotBnow = 0;

            foreach (var part in partYearEvents)
            {
                FullPriceNotBnow += part.PriceNotB;
            }
            return FullPriceNotBnow;
        }
        /// <summary>
        /// возвращает стоимость работ по учреждению бюджет
        /// </summary>
        /// <param name="agencyId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<float> GetFullPriceBNowAgency(int agencyId, AllContext db, string dataYear)
        {
            float FullPriceBnow = 0;
            List<YearEvent> yearEvents = await db.YearEvents.Where(y => y.AgencyId == agencyId)
                .Where(y => y.DataYear == dataYear)
                .ToListAsync();
            foreach(var ye in yearEvents)
            {
                FullPriceBnow += await GetFullPriceBNow(ye.Id, db);
            }
            return FullPriceBnow;
        }
        /// <summary>
        /// возвращает стоимость работ по учреждению внебюджет
        /// </summary>
        /// <param name="agencyId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<float> GetFullPriceNotBNowAgency(int agencyId, AllContext db, string dataYear)
        {
            float FullPriceNotBnow = 0;
            List<YearEvent> yearEvents = await db.YearEvents.Where(y => y.AgencyId == agencyId)
                .Where(y => y.DataYear == dataYear)
                .ToListAsync();
            foreach(var ye in yearEvents)
            {
                FullPriceNotBnow += await GetFullPriceNotBNow(ye.Id, db);
            }
            return FullPriceNotBnow;
        }
        public async Task<float> GetFullPriceBNowTotal(List<Agency> agencies, AllContext db, string dataYear)
        {
            float FullPriceBnow = 0;
            foreach (Agency agency in agencies)
            {
                FullPriceBnow += await GetFullPriceBNowAgency(agency.Id, db, dataYear);
            }
            return FullPriceBnow;
        }
        public async Task<float> GetFullPriceNotBNowTotal(List<Agency> agencies, AllContext db, string dataYear)
        {
            float FullPriceNotBnow = 0;
            foreach (Agency agency in agencies)
            {
                FullPriceNotBnow += await GetFullPriceNotBNowAgency(agency.Id, db, dataYear);
            }
            return FullPriceNotBnow;
        }

    }
}
