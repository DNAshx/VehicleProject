using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using GlauxSoft.Business;

using GlauxSoft.GreenTransport.Common;
using Evidence.Business;
using myConst = GlauxSoft.GreenTransport.Repository.Constants;


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

        /// <summary>
        /// Testet das Query GetPersonByCity, indem überprüft wird, dass die
        /// erwarteten Datensätze gefunden und zurückgegeben werden.
        /// </summary>
        [TestMethod]
        public void TestGetPersonByCity()
        {
            List<Person> personList = new List<Person>();
            EvdSession session = null;
            try
            {
                // 1. Setup: Mit evidence verbinden, Objekte erzeugen
                session = EvdSession.Logon("Administrator", "EvidenceXP", new EvdLangId(7),
                new Uri(@"tcp://localhost:15721/smartLbz"));
                SessionManager.Register(session);
                string city = Guid.NewGuid().ToString().Replace("-", ""); // eindeutiger Stadtname
                int numObjs = 5; // Anzahl Testobjekte: beliebig
                for (int i = 0; i < numObjs; i++)
                {
                    Person p = BusinessObject.Create<Person>();
                    p.FirstName = "Aaa";// RandomProvider.NextFirstName();
                    p.Nachname = "bb";// RandomProvider.NextLastName();
                    p.CityName = city;
                    p.Save();
                    personList.Add(p);
                }
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
            finally
            {
                // 4. Teardown: Objekte löschen, Session deregistrieren
                foreach (Person p in personList)
                {
                    p.Drop();
                }
                if (session != null)
                    SessionManager.Unregister();
            }
        }
    }
}
