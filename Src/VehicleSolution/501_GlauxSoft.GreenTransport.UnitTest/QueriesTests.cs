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
    /// <summary>
    /// Summary description for QueryTest
    /// </summary>
    [TestClass]
    public class QueryTest
    {
        public QueryTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
        }

        private static Properties.Settings settings = GlauxSoft.GreenTransport.UnitTest.Properties.Settings.Default;

        /// <summary>
        /// Testet das Query GetPersonByCity, indem überprüft wird, dass die
        /// erwarteten Datensätze gefunden und zurückgegeben werden.
        /// </summary>
        [TestMethod]
        public void TestGetPersonByCity()
        {
            //// 1. Setup: Mit evidence verbinden, Objekte erzeugen
            EvdSession session = EvdSession.Logon(settings.Username, settings.Password, new EvdLangId(7), new Uri(@"tcp://localhost:15719/vehicle"));
            SessionManager.Register(session);

            List<Person> personList = new List<Person>();

            try
            {
                //SessionHelper.Connect();
                var cntPersons = 10;
                for (int i = 0; i < cntPersons; i++)
                {
                    Person p = BusinessObject.Create<GlauxSoft.GreenTransport.Repository.Person>();

                    p.FirstName = RandomProvider.NextFirstName();
                    p.Nachname = RandomProvider.NextLastName();
                    p.CityName = RandomProvider.NextCity();
                    p.CityState = RandomProvider.NextStreet();
                    p.Save();
                    personList.Add(p);
                }


                //string city = Guid.NewGuid().ToString().Replace("-", ""); // eindeutiger Stadtname
                //int numObjs = 5; // Anzahl Testobjekte: beliebig

                //57
                // 2. Exercise: Die zu testende Methode ausführen
                //var query = QueryCollection.
                //    new GlauxSoft.TestProjekt.Common.Queries.PersonQueryAccessor();

                //var result =  query.GetPersonByCity(
                ////GlauxSoft.TestProjekt.Common.Queries.GetPersonByCity.GetObjects
                //// 3. Verify: Überprüfen dass die richtigen Resultate zurückgegeben werden
                //Assert.IsNotNull(result);
                //Assert.AreEqual(numObjs, result.Count); // gleiche anzahl an personen gefunden
                //foreach (Person p in personList)
                //{
                //    // IDs müssen übereinstimmen
                //    ////Person resPers = result.FirstOrDefault(rp => rp.ObjectID == p.ObjectID);
                //    //Assert.IsNotNull(resPers, string.Format(
                //    //"Person mit ID {0}, {1} {2}, nicht gefunden",
                //    //p.ObjectID, p.FirstName, p.Nachname));
                //}
            }
            catch// (Exception ex)
            {
                //session = null;
            }
            finally
            {
                // 4. Teardown: Objekte löschen, Session deregistrieren
                //foreach (Person p in personList)
                //{
                //    p.Drop();
                //}
                if (session != null)
                    SessionManager.Unregister();
            }
        }

    }
}