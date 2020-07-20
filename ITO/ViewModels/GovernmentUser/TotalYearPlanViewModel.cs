using ITO.Models;
using ITO.ViewModels.AgencyUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.ViewModels.GovernmentUser
{
    public class TotalYearPlanViewModel
    {
        public string DataYear { get; set; }
        public decimal Procent { get; set; }

        public int FullDonePlan { get; set; }
        public int NowDonePlan { get; set; }
        public float FullPriceBnow { get; set; }
        public float FullPriceNotBnow { get; set; }
        public float FullPrice { get; set; }
        public List<DataYear> DataYears { get; set; }
        public List<AgencyYearPlanViewModel> AgencyYearPlanViewModels { get; set; }
    }
}
