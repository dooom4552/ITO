using ITO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.ViewModels.Sections
{
    public class SubSectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SectionId { get; set; }//ТСО ИСОН  //надо класс +    
        public List<YearEvent> YearEvents { get; set; }
        public List<SubSection1> SubSection1s { get; set; }
    }
}
