using ITO.Interfaces;
using ITO.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.ViewModels.AgencyUser
{
    public class PartYearEventViewModel : PartYearEvent
    {    
        [NotMapped]
        [DisplayName("ImageFile")]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        [DisplayName("PdfFile")]
        public IFormFile PdfFile { get; set; }//ссылка на Pdf

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
        public string DataYear { get; set; }//
        public int maxDone { get; set; }//


    }
}
