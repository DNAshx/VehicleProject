
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using GlauxSoft.Business;
using GlauxSoft.GreenTransport.Common;
using Evidence.Business;
using myConst = GlauxSoft.GreenTransport.Repository.Constants;

namespace GlauxSoft.GreenTransport.DataImport
{
    class Program
    {
        static void Main(string[] args)
        {
        }
        
        private static void ImportPersons()
        {
            // xml einlesen (LINQ to xml)
            var xdoc = XDocument.Load("Data\\Persons.xml");
            var persons = from p in xdoc.Descendants("Person")
                           select new
                           {
                               FirstName = p.Element("FirstName").Value,
                               Nachname = p.Element("Nachname").Value,
                               Adresse = p.Element("Adresse").Value,
                               CityCode = p.Element("CityCode").Value,
                               CityName = p.Element("CityName").Value,
                               CityState = p.Element("CityState").Value,
                               CountryCode = p.Element("CountryCode").Value,
                               CountryName = p.Element("CountryName").Value,
                               EmailPrivat = p.Element("EmailPrivat").Value
                           };
            foreach (var p in persons)
            {
                var person = BusinessObject.Create<Person>();
                person.FirstName = p.FirstName;
                person.Nachname = p.Nachname;
                person.Adresse = p.Adresse;
                person.CityCode = p.CityCode;
                person.CityName = p.CityName;
                person.CityState = p.CityState;
                person.CountryCode = p.CountryCode;
                person.CountryName = p.CountryName;
                person.EmailPrivat = p.EmailPrivat;
                person.Save();
            }
        }
        private static void ImportVehicle()
        {
            // xml einlesen (LINQ to xml)
            var xdoc = XDocument.Load("Data\\Vehicles.xml");
            var vehicles = from p in xdoc.Descendants("Vehicle")
                          select new
                          {
                              Brand = p.Element("Brand").Value,
                              Class = p.Element("Class").Value,
                              CO2 = p.Element("CO2").Value,
                              Description = p.Element("Description").Value,
                              Efficiency = p.Element("Efficiency").Value,
                              Engine = p.Element("Engine").Value,
                              Feature = p.Element("Feature").Value,
                              Fuel = p.Element("Fuel").Value,
                              Location = p.Element("Location").Value,
                              Maintenance = p.Element("Maintenance").Value,
                              PriceDay = p.Element("PriceDay").Value,
                              ProdDate = p.Element("ProdDate").Value,
                              QtPassengers = p.Element("QtPassengers").Value,
                              ServiceHours = p.Element("ServiceHours").Value,
                              Type = p.Element("Type").Value,
                              Value = p.Element("Value").Value,
                          };
            foreach (var v in vehicles)
            {
                var vehicle = BusinessObject.Create<Vehicle>();
                vehicle.Brand = v.Brand;
                //vehicle.Class = v.Class;
                vehicle.CO2 = decimal.Parse( v.CO2);
                vehicle.Description  = v.Description;
                vehicle.Efficiency = v.Efficiency;
                vehicle.Engine = decimal.Parse(v.Engine);
                vehicle.Feature = v.Feature;
                
                vehicle.PriceDay = double.Parse( v.PriceDay);
                vehicle.ProdDate = DateTime.Parse( v.ProdDate);
                vehicle.QtPassengers = int.Parse(v.QtPassengers);
                vehicle.ServiceHours = int.Parse(v.ServiceHours);
                //vehicle.Type = v.Type;
                //vehicle.Value = v.Value;
                vehicle.Save();
            }
        }


        private static void ImportVehicleOrders()
        {
            // xml einlesen (LINQ to xml)
            var xdoc = XDocument.Load("Data\\VehicleOrders.xml");
            var vehicleOrders = from p in xdoc.Descendants("VehicleOrder")
                           select new
                           {
                               DateFr = p.Element("DateFr").Value,
                               DateTo = p.Element("DateTo").Value,
                               Status = p.Element("Status").Value,
                               Type = p.Element("Type").Value,
                               Amount = p.Element("Amount").Value
                           };
            foreach (var vo in vehicleOrders)
            {
                var vehicleOrder = BusinessObject.Create<VehicleOrder>();
                vehicleOrder.DateFrom = DateTime.Parse(vo.DateFr);
                vehicleOrder.DateTo = DateTime.Parse(vo.DateTo);
                vehicleOrder.Amount = decimal.Parse(vo.Amount);
                vehicleOrder.Save();
            }
        }
    }
}
