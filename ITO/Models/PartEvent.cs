using ITO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITO.Models
{
    public class PartEvent : IPrice, IPartDone, IDateTime, IUserNameSent
    {
        public float PriceB { get ; set ; }
        public float PriceNotB { get ; set ; }
        public int Done { get ; set ; }
        public DateTime DateTime { get ; set ; }
        public string UserNameSent { get ; set ; }
    }
}
