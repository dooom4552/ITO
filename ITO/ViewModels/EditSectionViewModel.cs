using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITO.Models;

namespace ITO.ViewModels
{
    public class EditSectionViewModel
    {
        public List<Unit> Units { get; set; }//мера
        public List<TypeSection> TypeSections { get; set; }
        public List<Section> Sections { get; set; }//ИСО ТСОН
        public List<SubSection> SubSections { get; set; }//Видео ППК
        public List<SubSection1> SubSection1s { get; set; }//Видео внутр  ТЭОИ
        public List<DataYear> DataYears { get; set; }
        public string Name { get; set; }
    }
}
