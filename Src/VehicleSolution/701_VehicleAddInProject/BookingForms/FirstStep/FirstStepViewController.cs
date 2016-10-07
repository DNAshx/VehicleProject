using System;
using System.IO;
using System.Collections.Generic;

using Evidence.Business;
using Evidence.Services;
using Evidence.Nova.Common;

using GlauxSoft.Common;
using GlauxSoft.GreenTransport.Repository;
using GlauxSoft.Business;
using GreenTransport.Controllers;

namespace GreenTransport.BookingForms.FirstStep
{
	[DefaultView(typeof(FirstStepView))]
    public class FirstStepViewController : BaseController<FirstStepViewModel>
    {        
        public ActionResult Index()
        {
            var view = GetView<FirstStepView>();
            var form = view.Root as NovaForm;            
            view.Contact.RefClassId = ((BusinessObject)(new Person())).ClassID;            
            view.GridVehicles.ItemsSource = ViewModel.VehicleList;            

            view.StartDate.Value = DateTime.Today;
            view.EndDate.Value = DateTime.Today.AddDays(1);
           
            //view.VehicleType.FillFromEnum(new EvidenceEnum(   ,GlauxSoft.GreenTransport.Repository.Enums.VehicleType.Bicycle,GlauxSoft.GreenTransport.Repository.Enums.VehicleType.Bicycle);
            //view.VehicleClass.FillFromEnum(new EvidenceEnum( ,GlauxSoft.GreenTransport.Repository.Enums.CarClass.Small,GlauxSoft.GreenTransport.Repository.Enums.CarClass.Small);
            
            //using (var stream = this.GetType().Assembly.GetManifestResourceStream("GreenTransport.Images.owls.jpg"))
            //{
            //    if (stream != null)
            //    {
            //        byte[] ba = new byte[stream.Length];
            //        stream.Read(ba, 0, ba.Length);
            //        view.Image.ImageContent.SetContent(ba, ".jpg");
            //    }
            //}

            
            if (form == null) throw new NovaException("Wrong type " + view.Root.GetType());
            form.Title = "Booking process.";

            return new DoNothingResult();
        }


        public ActionResult FormClose()
        {
            return new CloseDialogResult(true, null);
        }



        #region Wizzard Navigation

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
        public ActionResult FrmWizardBack()
        {
            var form = View.FindElementByName<NovaForm>("frmGreenTransportStartView");
            if (form != null && form.WizardCanGoBack())
            {
                form.WizardGoBack();
                ViewModel.CurrentPageNumber--;               
            }
            return new DoNothingResult();
        }

        public ActionResult FrmWizardCancel()
        {
            return new NavigateBackResult();
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

        #endregion

        #region helper

        private void InitGrid()
        {
            var view = GetView<FirstStepView>();
            var form = view.Root as NovaForm;
            var dateFr = view.StartDate.Value.GetValueOrDefault();
            var dateTo = view.EndDate.Value.GetValueOrDefault();
            //var vType = view.VehicleClass.SelectedItem.ItemId;
            Random cnt = new Random();
            var num = cnt.Next(10);
            ViewModel.VehicleList.Clear();
            for (int i = 0; i < num; ++i)
            {
                ViewModel.VehicleList.Add(new VehicleModelObject(
                    new Vehicle()
                    {
                        Brand = RandomProvider.NextCompany(),
                        QtPassengers = RandomProvider.NextInt(2, 6),
                        PriceDay = RandomProvider.NextDouble(5, 100)
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
        
        #endregion

        #region events
        public ActionResult SearchEvent()
        {
            InitGrid();
            return new DoNothingResult();
        }

        public ActionResult SelectContact()
        {
            var view = GetView<FirstStepView>();
            view.Contact.ActionAfterSelectedObjectChanged = "SelectContact";

            return new DoNothingResult();
        }
        #endregion
    }
}
