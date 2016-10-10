using Evidence.Nova.Common;
using GlauxSoft.GreenTransport.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenTransport.NovaForms.RentWizzard
{
    public sealed class PersonGridViewModel : ViewModelObject
    {
        public bool ToSelect { get; set; }
        public int PersonID { get; set; }
        public int CompanyID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PLZ { get; set; }
        public string City { get; set; }
        public string AddressType { get; set; }

        public PersonGridViewModel(Person p)
        {
            this.ToSelect = false;
            this.PersonID = int.Parse(p.ObjectID.ToString());
            this.CompanyID = int.Parse(p.Ref_Company.ToString());
            this.LastName = p.Nachname;
            this.FirstName = p.FirstName;
            this.Street = p.Adresse;
            this.HouseNumber = p.Hausnummer;
            this.PLZ = p.CountryCode;
            this.City = p.CityName;
            this.AddressType = "";// p.AddressingForm.ToString();
        }
    }
}
