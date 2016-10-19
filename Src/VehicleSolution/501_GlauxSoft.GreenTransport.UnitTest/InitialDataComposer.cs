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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GlauxSoft.GreenTransport.UnitTest
{
    [TestClass]
    public class InitialDataComposer
    {
        private static Properties.Settings settings = GlauxSoft.GreenTransport.UnitTest.Properties.Settings.Default;

        [TestMethod]
        public void CheckInitialData()
        {
            EvdSession session = null;
            try
            {

                session = EvdSession.Logon(settings.Username, settings.Password, new EvdLangId(7), new Uri(string.Format("tcp://localhost:{0}/{1}", settings.Port, settings.ServiceUri)));

                SessionManager.Register(session);
                // locations
                ComposeLocations();
                var locations = GlauxSoft.GreenTransport.Queries.QueryFactory.Location.LocationGetList.GetObjects<Location>();
                Assert.IsTrue(locations.Count > 0);
                // check Vehicles
                var vehicles = GlauxSoft.GreenTransport.Queries.QueryFactory.Vehicle.VehicleGetList.GetObjects<Vehicle>();
                if (vehicles.Count < 10)
                {
                    ComposeVehicles(10);
                }
                vehicles = GlauxSoft.GreenTransport.Queries.QueryFactory.Vehicle.VehicleGetList.GetObjects<Vehicle>();
                Assert.IsTrue(vehicles.Count > 0);
                // check orders
                var dateStart = DateTime.Now.AddDays(RandomProvider.NextInt(10, 100));
                var orders = GlauxSoft.GreenTransport.Queries.QueryFactory.VehicleOrder.VehicleOrderGetByDateRange.GetObjects<VehicleOrder>(dateStart, dateStart.AddDays(100));
                if (orders.Count < 5)
                {

                    var tmpDate = dateStart;
                    for (int i = 0; i < 10; ++i)
                    {
                        var days = RandomProvider.NextInt(5, 20);

                        ComposeVehicleOrder(tmpDate, tmpDate.AddDays(days - 1));
                        tmpDate = tmpDate.AddDays(days).Date;
                    }

                    orders = GlauxSoft.GreenTransport.Queries.QueryFactory.VehicleOrder.VehicleOrderGetByDateRange.GetObjects<VehicleOrder>(dateStart, dateStart.AddDays(100));
                    Assert.IsTrue(orders.Count > 0);
                }
            }
            catch// (Exception ex)
            {
                //session = null;
            }
            finally
            {
                if (session != null)
                    SessionManager.Unregister();
            }
        }

        internal void ComposeLocations()
        {
            // check locations
            var locations = GlauxSoft.GreenTransport.Queries.QueryFactory.Location.LocationGetList.GetObjects<Location>();
            if (locations.Count == 0)
            {
                var tmpLocations = new string[] { "Berne", "Zurich", "Basel" };
                foreach (var loc in tmpLocations)
                {
                    var location = new Location()
                    {
                        Name = loc,
                        Code = loc.ToUpper()
                    };
                    location.Save();
                }
            }
            locations = GlauxSoft.GreenTransport.Queries.QueryFactory.Location.LocationGetList.GetObjects<Location>();
            Assert.IsTrue(locations.Count > 0);
        }

        internal void ComposeVehicles(int cnt)
        {
            var listTypes = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.CarType.Electric, GlauxSoft.GreenTransport.Repository.Enums.CarType.Diesel, GlauxSoft.GreenTransport.Repository.Enums.CarType.Hybrid, GlauxSoft.GreenTransport.Repository.Enums.CarType.Petrol });
            var listClasses = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.CarClass.Small, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Medium, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Large, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Estate, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Premium, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Carriers, GlauxSoft.GreenTransport.Repository.Enums.CarClass.SUV });
            var listFuels = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.FuelType.Diesel, GlauxSoft.GreenTransport.Repository.Enums.FuelType.Gasoline, GlauxSoft.GreenTransport.Repository.Enums.FuelType.LPG, GlauxSoft.GreenTransport.Repository.Enums.FuelType.V95, GlauxSoft.GreenTransport.Repository.Enums.FuelType.V98 });
            var locations = GlauxSoft.GreenTransport.Queries.QueryFactory.Location.LocationGetList.GetObjects<Location>();
            for (int i = 0; i < cnt; i++)
            {
                Vehicle v = BusinessObject.Create<GlauxSoft.GreenTransport.Repository.Vehicle>();
                v.Brand = RandomProvider.NextCompany();
                v.Class = listClasses[RandomProvider.NextInt(0, listClasses.Count - 1)];
                v.CO2 = RandomProvider.NextDecimal(0.1M, 2M);
                v.Description = string.Format("{0}-{1}", v.Brand, RandomProvider.NextFirstName());
                v.Efficiency = "22";
                v.Engine = RandomProvider.NextDecimal(1, 3);
                v.Fuel = listFuels[RandomProvider.NextInt(0, listFuels.Count - 1)];
                var location = locations[RandomProvider.NextInt(0, locations.Count - 1)];
                v.RefLocation = location.ObjectID;
                v.PriceDay = RandomProvider.NextDouble(25, 200);
                v.ProdDate = DateTime.Now.AddYears(-RandomProvider.NextInt(1, 3)).AddMonths(-RandomProvider.NextInt(1, 12)).Date;
                v.QtPassengers = RandomProvider.NextInt(2, 5);
                v.ServiceHours = RandomProvider.NextInt(2, 10);
                v.Type = listTypes[RandomProvider.NextInt(0, listTypes.Count - 1)];
                v.Value = RandomProvider.NextDouble(10, 100);
                v.Save();
                // personList.Add(p);
            }
        }

        internal void ComposeVehicleOrder(DateTime fr, DateTime to)
        {
            var listTypes = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.CarType.Electric, GlauxSoft.GreenTransport.Repository.Enums.CarType.Diesel, GlauxSoft.GreenTransport.Repository.Enums.CarType.Hybrid, GlauxSoft.GreenTransport.Repository.Enums.CarType.Petrol });
            var listClasses = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.CarClass.Small, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Medium, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Large, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Estate, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Premium, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Carriers, GlauxSoft.GreenTransport.Repository.Enums.CarClass.SUV });
            var listFuels = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.FuelType.Diesel, GlauxSoft.GreenTransport.Repository.Enums.FuelType.Gasoline, GlauxSoft.GreenTransport.Repository.Enums.FuelType.LPG, GlauxSoft.GreenTransport.Repository.Enums.FuelType.V95, GlauxSoft.GreenTransport.Repository.Enums.FuelType.V98 });
            var locations = GlauxSoft.GreenTransport.Queries.QueryFactory.Location.LocationGetList.GetObjects<Location>();
            var vehicles = GlauxSoft.GreenTransport.Queries.QueryFactory.Vehicle.VehicleGetList.GetObjects<Vehicle>();
            var persons = GlauxSoft.GreenTransport.Queries.QueryFactory.Person.SearchPerson.GetObjects<Person>("Em");
            //var companies = GlauxSoft.GreenTransport.Queries.QueryFactory.Vehicle.VehicleGetList.GetObjects<Person>();
            Assert.IsTrue(locations.Count > 0);
            Assert.IsTrue(vehicles.Count > 0);
            Assert.IsTrue(persons.Count > 0);

            VehicleOrder order = BusinessObject.Create<GlauxSoft.GreenTransport.Repository.VehicleOrder>();
            order.Amount = RandomProvider.NextDecimal(200, 500);
            order.DateFrom = fr;
            order.DateTo = to;
            order.FuelQNStart = RandomProvider.NextInt(1, 4);
            order.FuelQNClose = RandomProvider.NextInt(1, 4);
            order.OrderType = GlauxSoft.GreenTransport.Repository.Enums.OrderType.Booking;
            order.OrderStatus = GlauxSoft.GreenTransport.Repository.Enums.OrderStatus.Active;
            order.RefPerson = persons[RandomProvider.NextInt(0, persons.Count - 1)].ObjectID;
            order.RefVehicle = vehicles[RandomProvider.NextInt(0, vehicles.Count - 1)].ObjectID;
            order.Save();
        }
    }
}