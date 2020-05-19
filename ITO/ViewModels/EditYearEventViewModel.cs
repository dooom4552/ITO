using ITO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.ViewModels
{
    public class EditYearEventViewModel
    {
        public int Id { get; set; }
        public Agency Agency { get; set; }
        public int AgencyId { get; set; }
        public int Number { get; set; }
        public string EventText { get; set; }

        public int FirstQuarter { get; set; }
        public int SecondQuarter { get; set; }
        public int ThirdQuarter { get; set; }
        public int FourthQuarter { get; set; }

        public string Unit { get; set; }//мера
        public List<Unit> Units { get; set; }//мера

        public string TypeSection { get; set; }//мера
        public List<TypeSection> TypeSections { get; set; }//ремонт как ремонт

        public int SectionId { get; set; }//ремонт как ремонт
        public List<Section> Sections { get; set; }//ИСО ТСОН

        public int SubSectionId { get; set; }//ИСО ТСОН
        public string SubSection { get; set; }//ИСО ТСОН 
        public List<SubSection> SubSections { get; set; }//Видео ППК

        public int SubSection1Id { get; set; }//Видео ППК
        public string SubSection1 { get; set; }//Видео ППК
        public List<SubSection1> SubSection1s { get; set; }//Видео внутр  ТЭОИ

        public string DataYear { get; set; }//
        public List<DataYear> DataYears { get; set; }//Видео внутр  ТЭОИ
    }
}
