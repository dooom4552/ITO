using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.Interfaces
{
    interface IViewModelPartYearEvent
    {
        public int YearEventId { get; set; }
        public int NumberYearEvent { get; set; }// порядковый номер в год плане
        public int Done { get; set; }// сколько сделано
        public string Img { get; set; }//ссылка на картинку
        public string Pdf { get; set; }//ссылка на Pdf
        public float PriceB { get; set; }
        public float PriceNotB { get; set; }
        public DateTime DateTime { get; set; }// время выполнения
        public string UserNameСonfirmed { get; set; }// кто подтвердил  
        public string UserNameSent { get; set; }// кто отправил
        public string Сomment { get; set; }// кто отправил
    }
}
