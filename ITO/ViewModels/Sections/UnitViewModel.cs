using ITO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.ViewModels.Sections
{
    public class UnitViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<YearEvent> YearEvents { get; set; }
    }
}
