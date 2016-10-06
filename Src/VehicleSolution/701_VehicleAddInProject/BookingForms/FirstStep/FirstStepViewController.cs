using System;
using System.IO;
using System.Collections.Generic;

using Evidence.Business;
using Evidence.Services;
using Evidence.Nova.Common;

using GlauxSoft.Common;
using GlauxSoft.GreenTransport.Repository;
using GlauxSoft.Business;

namespace GreenTransport.BookingForms.FirstStep
{
	[DefaultView(typeof(FirstStepView))]
    public class FirstStepViewController : ControllerBase<FirstStepViewModel>
    {        
        public ActionResult Index()
        {
            var view = GetView<FirstStepView>();
            var form = view.Root as NovaForm;
            //view.OrderName = string.Format("Order No. {0}", new Random().Next(1, 100));
            view.Contact.RefClassId = ((BusinessObject)(new Person())).ClassID;
            view.Vehicle.RefClassId = ((BusinessObject)(new Vehicle())).ClassID;
            view.GridVehicles.ItemsSource = ViewModel.VehicleList;

            #region ///add contacts
            //Random houseNo = new Random();
            //Random countryCode = new Random(1000);
            //Person p = BusinessObject.Create<Person>();

            //p.FirstName = RandomProvider.NextFirstName();
            //p.Nachname = RandomProvider.NextLastName();
            //p.CityName = RandomProvider.NextCity();
            //p.TelefonPrivat = RandomProvider.NextPhoneNumber();
            //p.Adresse = RandomProvider.NextStreet();
            //p.CountryCode = countryCode.Next(4000).ToString();
            //p.Hausnummer = houseNo.Next(1, 40).ToString();
            //p.Save();
            //view.Contact.RefClassId = p.ClassID;
            //view.Contact.IsRequired = true;
            #endregion

            view.StartDate.Value = DateTime.Today;
            view.EndDate.Value = DateTime.Today.AddDays(1);
            //GlauxSoft.GreenTransport.Repository.Enums.CarClass
            //Vehicle v;
           
            //view.VehicleType.FillFromEnum(new EvidenceEnum(   ,GlauxSoft.GreenTransport.Repository.Enums.VehicleType.Bicycle,GlauxSoft.GreenTransport.Repository.Enums.VehicleType.Bicycle);
            //view.VehicleClass.FillFromEnum(new EvidenceEnum( ,GlauxSoft.GreenTransport.Repository.Enums.CarClass.Small,GlauxSoft.GreenTransport.Repository.Enums.CarClass.Small);
            
            using (var stream = this.GetType().Assembly.GetManifestResourceStream("GreenTransport.Images.owls.jpg"))
            {
                if (stream != null)
                {
                    byte[] ba = new byte[stream.Length];
                    stream.Read(ba, 0, ba.Length);
                    view.Image.ImageContent.SetContent(ba, ".jpg");
                }
            }

            
            if (form == null) throw new NovaException("Wrong type " + view.Root.GetType());
            form.Title = "Booking process.";

            return new DoNothingResult();//NovaMessageBoxDialog()
            //{
            //    Text = "Hello Nova AddIn!",
            //    Title = "Hello Nova AddIn!",
            //    Buttons = NovaMessageBoxButtons.Ok,
            //    DefaultButton = NovaMessageBoxButtonDefault.No,
            //    Icon = NovaMessageBoxIcon.Info
            //};
        }


        public ActionResult FormClose()
        {
            return new CloseDialogResult(true, null);
        }

        public ActionResult FrmWizardNext()
        {
            var form = View.FindElementByName<NovaForm>("frmGreenTransportStartView");
            if (form != null && form.WizardCanGoNext())
            {
                form.WizardGoNext();
                ViewModel.CurrentPageNumber++;                
            }
            return new DoNothingResult();
        }

        public ActionResult FrmWizardFinish()
        {
            //var order = BusinessObject.Create<VehicleOrder>();
            //order.Amount = 
            return new NovaMessageBoxDialog()
                {
                    Text = "Bucking order created successeful!",
                    Title = "Success!",
                    Buttons = NovaMessageBoxButtons.Ok,
                    DefaultButton = NovaMessageBoxButtonDefault.No,
                    Icon = NovaMessageBoxIcon.Info
                };
        }
        /// <summary>
        /// Call the FormSave-function to save the evidence object
        /// </summary>
        /// <returns></returns>
        public ActionResult FormSave()
        {            
            return new DoNothingResult();
        }

        private void InitGrid()
        {
            var view = GetView<FirstStepView>();
            var form = view.Root as NovaForm;
            var dateFr = view.StartDate.Value;
            var dateTo = view.EndDate.Value;
            //var vType = view.VehicleClass.SelectedItem.ItemId;
            Random cnt = new Random();
            var num = cnt.Next(10);
            for (int i = 0; i < num; ++i)
            {
                ViewModel.VehicleList.Add(new VehicleModelObject(
                    new Vehicle()
                    {
                        Brand = RandomProvider.NextCompany(),
                        QtPassengers = RandomProvider.NextInt(1, 6),
                        PriceDay = RandomProvider.NextDouble(1, 100)
                    }));
            }
            //Random countryCode = new Random(1000);
            //Person p = BusinessObject.Create<Person>();

            //p.FirstName = RandomProvider.NextFirstName();
            
            //var filterName = View.SearchField;
            //var fctext = string.IsNullOrWhiteSpace(filterName.Text) ? string.Empty : filterName.Text.Trim();

            //if (ViewModel.IsToPerson)
            //{
            //    var allP = Common.Queries.QueryFactory.Person.GetAllPersons.GetObjects<VehicleOrder>();
            //    if (allP == null)
            //    {
            //        ViewModel.RelationList.Clear();
            //        return;
            //    }

            //    if (!string.IsNullOrWhiteSpace(fctext))
            //    {
            //        for (int i = 0; i < allP.Count; ++i)
            //        {
            //            Person p = allP[i];
            //            var pFirstName = p.NameFirst != null ? p.NameFirst.Trim().ToLower() : string.Empty;
            //            var pLastName = p.NameLast != null ? p.NameLast.Trim().ToLower() : string.Empty;

            //            bool ok1 = pFirstName.StartsWith(fctext.Trim().ToLower());
            //            bool ok2 = pLastName.StartsWith(fctext.Trim().ToLower());

            //            if (!ok1 && !ok2)
            //            {
            //                allP.RemoveAt(i);
            //                --i;
            //            }
            //        }
            //    }

            //    if (!string.IsNullOrWhiteSpace(ViewModel.ExcludePersonID))
            //    {
            //        var excludeIDs = ViewModel.ExcludePersonID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //        foreach (string rid in excludeIDs)
            //        {
            //            int excID;
            //            int.TryParse(rid, out excID);
            //            if (excID <= 0)
            //                continue;

            //            var excP = allP.FirstOrDefault(x => x.ObjectID.GetValue() == excID);
            //            if (excP != null)
            //                allP.Remove(excP);
            //        }
            //    }

            //    foreach (Person person in allP)
            //    {
            //        if (person != null)
            //        {
            //            var nachname = person.NameLast;
            //            var vorname = person.NameFirst;
            //            var mainAddress = !person.RefMainAddress.IsNull()
            //                ? CoreBusinessObject.GetObjects<Address>(new List<EvdObjectId> { person.RefMainAddress }).FirstOrDefault()
            //                : null;
            //            var street = mainAddress != null ? mainAddress.Street : string.Empty;
            //            var houseNumber = mainAddress != null ? mainAddress.StreetNo : string.Empty;
            //            var plz = mainAddress != null ? mainAddress.CityZIPCode : string.Empty;
            //            var ort = mainAddress != null ? mainAddress.City : string.Empty;
            //            var addressType = mainAddress != null ? mainAddress.AddressType : null;

            //            ViewModel.RelationList.Add(new RelationGridViewModel(0,
            //                    person.ObjectID.GetValue(),
            //                    0,
            //                    string.Empty,
            //                    nachname,
            //                    vorname,
            //                    street,
            //                    houseNumber,
            //                    plz,
            //                    ort,
            //                    string.Empty,
            //                    addressType));
            //        }
            //    }
            //}

            //if (ViewModel.IsToCompany)
            //{
            //    var allP = Common.Queries.QueryFactory.Company.GetAllCompanies.GetObjects<Company>();
            //    if (allP == null)
            //    {
            //        ViewModel.RelationList.Clear();
            //        return;
            //    }

            //    if (!string.IsNullOrWhiteSpace(fctext))
            //    {
            //        for (int i = 0; i < allP.Count; ++i)
            //        {
            //            Company p = allP[i];
            //            var pName = p.Name != null ? p.Name.Trim().ToLower() : string.Empty;
            //            var pName2 = p.Name2 != null ? p.Name2.Trim().ToLower() : string.Empty;

            //            bool ok1 = pName.StartsWith(fctext.Trim().ToLower());
            //            bool ok2 = pName2.StartsWith(fctext.Trim().ToLower());

            //            if (!ok1 && !ok2)
            //            {
            //                allP.RemoveAt(i);
            //                --i;
            //            }
            //        }
            //    }

            //    if (!string.IsNullOrWhiteSpace(ViewModel.ExcludeCompanyID))
            //    {
            //        var excludeIDs = ViewModel.ExcludeCompanyID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //        foreach (string rid in excludeIDs)
            //        {
            //            int excID;
            //            int.TryParse(rid, out excID);
            //            if (excID <= 0)
            //                continue;

            //            var excP = allP.FirstOrDefault(x => x.ObjectID.GetValue() == excID);
            //            if (excP != null)
            //                allP.Remove(excP);
            //        }
            //    }

            //    foreach (Company company in allP)
            //    {
            //        if (company != null)
            //        {
            //            var name = company.Name;
            //            var nameZusatz = company.Name2;
            //            var mainAddress = !company.RefMainAddress.IsNull()
            //                ? CoreBusinessObject.GetObjects<Address>(new List<EvdObjectId> { company.RefMainAddress }).FirstOrDefault()
            //                : null;
            //            var street = mainAddress != null ? mainAddress.Street : string.Empty;
            //            var houseNumber = mainAddress != null ? mainAddress.StreetNo : string.Empty;
            //            var plz = mainAddress != null ? mainAddress.CityZIPCode : string.Empty;
            //            var ort = mainAddress != null ? mainAddress.City : string.Empty;
            //            var addressType = mainAddress != null ? mainAddress.AddressType : null;

            //            ViewModel.RelationList.Add(new RelationGridViewModel(0,
            //                    0,
            //                    company.ObjectID.GetValue(),
            //                    string.Empty,
            //                    name,
            //                    nameZusatz,
            //                    street,
            //                    houseNumber,
            //                    plz,
            //                    ort,
            //                    string.Empty,
            //                    addressType));
            //        }
            //    }
            //}
        }

        public ActionResult SearchEvent()
        {
            InitGrid();
            return new DoNothingResult();
        }

              
    }
}
