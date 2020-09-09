using ITO.Interfaces;
using ITO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.ViewModels.DivisionEnent
{
    public class PartYearEventDivisionVM : PartYearEvent, ISection
    {

        public string Section { get ; set ;}
        public string SubSection { get; set;}
        public string SubSection1 { get; set;}
        public string TypeSection { get ; set ;}
        public string Unit { get ; set ; }
    }
}
