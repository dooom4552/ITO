using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.ViewModels.AgencyUser
{/// <summary>
/// подробное конеретное мероприятие годового плана
/// </summary>
    public class PartYearEventDetailsViewModel
    {
        public int Id { get; set; }
        public string Agency { get; set; }//учреждение
        public string DataYear { get; set; }// год годового плана
        public string EventText { get; set; }// текст годового плана
        public int NumberYearEvent { get; set; }// порядковый номер в год плане
        public int Done { get; set; }// сколько сделано
        public int FullDonePlan { get; set; }// сколько запланировано сделать
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
