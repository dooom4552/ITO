using ITO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.ViewModels.AgencyUser
{
    public class AgencyYearPlanViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DataYear { get; set; }
        public decimal Procent { get; set; }
        public int FullDonePlan { get; set; }
        public int NowDonePlan { get; set; }
        public float FullPriceBnow { get; set; }
        public float FullPriceNotBnow { get; set; }
        public List<DataYear> DataYears { get; set; }
        public List<YearEventViewModel> YearEventViewModels { get; set; }
    }
}
