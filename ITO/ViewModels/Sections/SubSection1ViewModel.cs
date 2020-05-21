using ITO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.ViewModels.Sections
{
    public class SubSection1ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubSectionId { get; set; }//ТСО ИСОН  //надо класс +    
        public List<YearEvent> YearEvents { get; set; }
    }
}
