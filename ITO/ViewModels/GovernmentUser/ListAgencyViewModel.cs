using ITO.Models;
using ITO.ViewModels.AgencyUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.ViewModels.GovernmentUser
{
    public class ListAgencyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FullDonePlan { get; set; }
        public int NowDonePlan { get; set; }

        public  List<YearEventViewModel> YearEventViewModels { get; set; }
    }
}
