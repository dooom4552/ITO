using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.Models
{
    public class YearEvent
    {
        public int Id { get; set; }
        public Agency Agency { get; set; }
        public int Number { get; set; }// номер пункта в годовом плане
        public string EventText { get; set; }
        public int FirstQuarter { get; set; }
        public int SecondQuarter { get; set; }
        public int ThirdQuarter { get; set; }
        public int FourthQuarter { get; set; }
        public Unit Unit { get; set; }//мера
        public List< Unit> Units { get; set; }//мера
        public List<PartYearEvent> PartYearEvents { get; set; }// коллекция выполненных мероприятий
    }
}
