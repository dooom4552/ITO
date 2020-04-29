using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.Models
{
    public class PartYearEvent
    {
        public int Id { get; set; }
        public Agency Agency { get; set; }
        public int Number { get; set; }
        public int Done { get; set; }// сколько сделано
        public string Img { get; set; }//ссылка на картинку
        public string Pdf { get; set; }//ссылка на Pdf
        public float PriceB { get; set; }
        public float PriceNotB { get; set; }
        public DateTime DateTime { get; set; }// время выполнения
        public User Userconfirmed { get; set; }// кто подтвердил
    }
}
