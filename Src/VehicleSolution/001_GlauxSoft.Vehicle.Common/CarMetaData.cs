using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

using Evidence.Business;
using Evidence.Services;

using GlauxSoft.Common;
using GlauxSoft.GreenTransport.Repository;
using GlauxSoft.Business;


using myConst = GlauxSoft.GreenTransport.Repository.Constants;


namespace GlauxSoft.GreenTransport.Common
{
    public class CarMetaData
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        [EvidenceAttribute("Class")]
        public EvdEnumValue Class { get; set; }
        public decimal CO2 {get;set;}
        public string Description {get;set;}
        public string Efficiency {get;set;}
        public string Fuel {get;set;}
        public decimal Engine {get;set;}
        public decimal PriceDay {get;set;}
        public int QtPassengers {get;set;}
        public int ServiceHours {get;set;}
    }
}
