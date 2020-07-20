using ITO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.services
{
    public class AgencyFilter
    {
        public async Task<List<Agency>> GetAgenciesFilterYearEvent(List<Agency> agencies, AllContext db)
        {
            List<Agency> agenciesFilter = new List<Agency>();

            foreach (Agency ag in agencies)
            {
                List<YearEvent> yearEvents = await db.YearEvents.Where(y => y.AgencyId == ag.Id).ToListAsync();
                if (yearEvents.Count != 0)
                {
                    agenciesFilter.Add(ag);
                }
            }
            return agenciesFilter;
        }

        public static List<Agency> GetAgenciesFilterYearEvent(List<Agency> agencies, List<YearEvent> yearEvents)
        {
            List<Agency> agenciesFilter = new List<Agency>();
            foreach (Agency ag in agencies)
            {
                List<YearEvent> yearEventsFilter = yearEvents.Where(y => y.AgencyId == ag.Id).ToList();
                if (yearEventsFilter.Count != 0)
                {
                    agenciesFilter.Add(ag);
                }
            }
            return agenciesFilter;
        }



    }
}
