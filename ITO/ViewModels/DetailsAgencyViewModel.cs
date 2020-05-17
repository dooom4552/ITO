using ITO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.ViewModels
{
    public class DetailsAgencyViewModel
    {
        public int AgencyId { get; set; }
        public Agency Agency { get; set; }
        public List <YearEvent> YearEvents { get; set; }
        public List <DataYear> DataYears { get; set; }       
        public string DataYearVM { get; set; }       
    }
}
