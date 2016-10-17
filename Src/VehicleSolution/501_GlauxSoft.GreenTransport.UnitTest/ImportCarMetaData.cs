using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using Evidence.Business;
using Evidence.Services;

using GlauxSoft.Common;
using GlauxSoft.GreenTransport.Repository;
using GlauxSoft.GreenTransport.Common;
using GlauxSoft.Business;


using myConst = GlauxSoft.GreenTransport.Repository.Constants;

namespace GlauxSoft.GreenTransport.UnitTest
{
    internal class VehicleData
    {
        internal static List<CarMetaData> List { get; private set; }
        
        static void ProcessImport()
        {
            var xdoc = XDocument.Load("Data\\CarMetaData.xml");
            VehicleData.List = (from p in xdoc.Descendants("CarMetaData")
                          select new CarMetaData
                          {
                              Brand = p.Element("Brand").Value,
                              CO2 = decimal.Parse(p.Element("CO2").Value),
                              Description = p.Element("Description").Value,
                              Efficiency = p.Element("Efficiency").Value,
                              Engine = decimal.Parse(p.Element("Engine").Value),
                              Fuel = p.Element("Fuel").Value, //GlauxSoft.GreenTransport.Repository.Enums.FuelType.Diesel,
                              Model = p.Element("Model").Value,
                              PriceDay = decimal.Parse(p.Element("PriceDay").Value),
                              QtPassengers = int.Parse(p.Element("QtPassengers").Value),
                              ServiceHours = int.Parse(p.Element("ServiceHours").Value)
                          }).ToList();
        }
    }
}
