using Evidence.Business;
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
        public bool ToSelect
        {
            get { return (bool)GetValue(); }
            set { SetValue(value); }
        }
        public EvdObjectId PersonID
        {
            get { return (EvdObjectId)GetValue(); }
            set { SetValue(value); }
        }
        public int CompanyID
        {
            get { return (int)GetValue(); }
            set { SetValue(value); }
        }
        public string LastName
        {
            get { return (string)GetValue(); }
            set { SetValue(value); }
        }
        public string FirstName
        {
            get { return (string)GetValue(); }
            set { SetValue(value); }
        }
        public string Street
        {
            get { return (string)GetValue(); }
            set { SetValue(value); }
        }
        public string HouseNumber
        {
            get { return (string)GetValue(); }
            set { SetValue(value); }
        }
        public string PLZ
        {
            get { return (string)GetValue(); }
            set { SetValue(value); }
        }
        public string City
        {
            get { return (string)GetValue(); }
            set { SetValue(value); }
        }
        public string AddressType
        {
            get { return (string)GetValue(); }
            set { SetValue(value); }
        }        

        public PersonGridViewModel(Person p)
        {            
            this.ToSelect = false;
            this.PersonID = p.ObjectID;
            this.CompanyID = int.Parse(p.Ref_Company.ToString());
            this.LastName = p.Nachname.ToString();
            this.FirstName = p.FirstName;
            this.Street = p.Adresse;
            this.HouseNumber = p.Hausnummer;
            this.PLZ = p.CountryCode;
            this.City = p.CityName;
            this.AddressType = "";// p.AddressingForm.ToString();
        }
    }
}
