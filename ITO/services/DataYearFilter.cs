using ITO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.services
{
    public class DataYearFilter
    {/// <summary>
    /// Возвращает список объектов(годовые планы) по которым в данном учреждении заплвнированы мероприятия по ид
    /// </summary>
    /// <param name="agencyId"></param>
    /// <param name="db"></param>
    /// <returns></returns>
       static public async Task<List<DataYear>> GetDataYear(int agencyId, AllContext db)
        {
            List<YearEvent> yearEvents = new List<YearEvent>();
            yearEvents = await db.YearEvents.Where(y => y.AgencyId == agencyId).ToListAsync();
            List<DataYear> dataYears = new List<DataYear>();

            foreach(YearEvent yearEvent in yearEvents)
            {
                dataYears.Add(await db.DataYears.FirstOrDefaultAsync(d => d.Name == yearEvent.DataYear));
            }
            dataYears =  dataYears.Distinct().ToList();
            return dataYears;
        }
        /// <summary>
        /// возвращает имя годового плана по ид пункта годового плана
        /// </summary>
        /// <param name="YearEventId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        static public async Task<string> GetStringDataYear(int YearEventId, AllContext db) 
        {
            YearEvent yearEvent = await db.YearEvents.FirstOrDefaultAsync(y => y.Id == YearEventId);
            string hh= yearEvent.DataYear;
            return hh;
        }
    }
}
