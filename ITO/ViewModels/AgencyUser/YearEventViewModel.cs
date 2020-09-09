using ITO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.ViewModels.AgencyUser
{
    public class YearEventViewModel
    {
        public int Id { get; set; }
        public int AgencyId { get; set; }
        public int Number { get; set; }// номер пункта в годовом плане
        public string EventText { get; set; }
        public int FirstQuarter { get; set; }
        public int SecondQuarter { get; set; }
        public int ThirdQuarter { get; set; }
        public int FourthQuarter { get; set; }
        public string Unit { get; set; }//мера                        
        public string Section { get; set; }//ТСО ИСОН  //надо класс +                
        public string SubSection { get; set; }//мера    ППК или огр или ППЗ    // видео или ОИ  //надо класс   +            
        public string SubSection1 { get; set; }//мера  огр ОО ЭО // //надо класс 
        public string TypeSection { get; set; }//ремонт, замена, кап ремонт //надо класс
        public string DataYear { get; set; }
        public List<PartYearEvent> PartYearEvents { get; set; }//коллекция отчетов
        public decimal Procent { get; set; }
        public int NowDone { get; set; }
        public int FullDonePlan { get; set; }
        public float FullPriceBnow { get; set; }
        public float FullPriceNotBnow { get; set; }

        public bool NumberPartReturnsandSent { get; set; }
        public List<DataYear> DataYears { get; set; }
        public string TrClass { get; set; }

    }
}
