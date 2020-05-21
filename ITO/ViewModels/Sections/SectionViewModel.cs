using ITO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.ViewModels.Sections
{
    public class SectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<YearEvent> YearEvents { get; set; }
        public List<SubSection> SubSections { get; set; }
    }
}
