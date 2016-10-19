using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

using Evidence.Business;
using Evidence.Services;
using Evidence.Nova.Common;

using GlauxSoft.Common;
using GlauxSoft.GreenTransport.Repository;
using GlauxSoft.Business;
using GreenTransport.Controllers;

using eDocGenDocumentCreator;

using myConst = GlauxSoft.GreenTransport.Repository.Constants;

namespace GreenTransport.NovaForms.RentWizzard
{
	[DefaultView(typeof(FirstStepView))]
    public class FirstStepViewController : BaseController<FirstStepViewModel>
    {
        public FirstStepView CurrentView
        {
            get { return GetView<FirstStepView>(); }
        }

        public ActionResult Index()
        {
            CurrentView.Contact.RefClassId = BusinessDirectory.get_ClassDescriptor(myConst.Person.CLASSNAME).ID;
            CurrentView.Order.RefClassId = BusinessDirectory.get_ClassDescriptor(myConst.VehicleOrder.CLASSNAME).ID;
            CurrentView.GridVehicles.ItemsSource = ViewModel.VehicleList;
            CurrentView.GridPersons.ItemsSource = ViewModel.PersonList;

            CurrentView.StartDate.Value = DateTime.Today;
            CurrentView.EndDate.Value = DateTime.Today.AddDays(1);

            var listTypes = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.CarType.Electric, GlauxSoft.GreenTransport.Repository.Enums.CarType.Diesel, GlauxSoft.GreenTransport.Repository.Enums.CarType.Hybrid, GlauxSoft.GreenTransport.Repository.Enums.CarType.Petrol });
            var listClasses = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.CarClass.Small, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Medium, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Large, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Estate, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Premium, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Carriers, GlauxSoft.GreenTransport.Repository.Enums.CarClass.SUV });

            InitCombobox(CurrentView.VehicleType, listTypes);
            InitCombobox(CurrentView.VehicleClass, listClasses);

            //ComposeData();
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

            var form = CurrentView.Root as NovaForm;
            if (form == null) throw new NovaException("Wrong type " + form.GetType());
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
            if (ViewModel.CurrentStep == FirstStepViewModel.WizzardSteps.Order)
            {
                CreateOrder();                
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
            SaveOrder();
            PrintEForm();            
            
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
            if (ViewModel.CurrentStep == FirstStepViewModel.WizzardSteps.Vehicle)
            {
                var dateFr = CurrentView.StartDate.Value.GetValueOrDefault();
                var dateTo = CurrentView.EndDate.Value.GetValueOrDefault();
                //var vType = CurrentView.VehicleClass.SelectedItem.ItemId;
                Random cnt = new Random();
                var num = cnt.Next(10);
                ViewModel.VehicleList.Clear();

                var orders = GlauxSoft.GreenTransport.Queries.QueryFactory.VehicleOrder.VehicleOrderGetByDateRange.GetObjects<VehicleOrder>(dateFr, dateTo);
                var vehicles = GlauxSoft.GreenTransport.Queries.QueryFactory.Vehicle.VehicleGetList.GetObjects<Vehicle>();
                foreach (var data in vehicles)
                {
                    var order = orders.FirstOrDefault(p => p.RefVehicle == data.ObjectID);
                    if (order == null)
                    {
                        ViewModel.VehicleList.Add(new VehicleModelObject(
                            new Vehicle()
                            {
                                Brand = data.Brand,
                                QtPassengers = data.QtPassengers,
                                PriceDay = data.PriceDay
                            }));
                    }
                }
            }
            else if (ViewModel.CurrentStep == FirstStepViewModel.WizzardSteps.Person)
            {
                var filterName = CurrentView.SearchPersonField;
                var fctext = string.IsNullOrWhiteSpace(filterName.Text) ? string.Empty : filterName.Text.Trim();
                
                ViewModel.PersonList.Clear();
                if (!string.IsNullOrEmpty(fctext))
                {
                    var allP = GlauxSoft.GreenTransport.Queries.QueryFactory.Person.SearchPerson.GetObjects<Person>(fctext);

                    if (!string.IsNullOrWhiteSpace(fctext))
                    {
                        //for (int i = 0; i < allP.Count; ++i)
                        //{
                        //    Person p = allP[i];
                        //    var pFirstName = p.FirstName != null ? p.FirstName.Trim().ToLower() : string.Empty;
                        //    var pLastName = p.Nachname != null ? p.Nachname.Trim().ToLower() : string.Empty;

                        //    bool ok1 = pFirstName.StartsWith(fctext.Trim().ToLower());
                        //    bool ok2 = pLastName.StartsWith(fctext.Trim().ToLower());

                        //    if (!ok1 && !ok2)
                        //    {
                        //        allP.RemoveAt(i);
                        //        --i;
                        //    }
                        //}
                    }
                    foreach (Person person in allP)
                    {
                        if (person != null)
                        {
                            ViewModel.PersonList.Add(new PersonGridViewModel(person));
                        }
                    }
                }
            }
            
        }

        private void CreateOrder()
        {
            CurrentView.OrderName = "Order_" + (new Random()).Next(10, 1000);            
            CurrentView.DateFrom.Value = CurrentView.StartDate.Value;
            CurrentView.DateTo.Value = CurrentView.EndDate.Value;
            CurrentView.Amount.Value = 1234.44m;
            var listTypes = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.OrderType.Booking, GlauxSoft.GreenTransport.Repository.Enums.OrderType.Holding, GlauxSoft.GreenTransport.Repository.Enums.OrderType.Service});            
            InitCombobox(CurrentView.OrderType, listTypes);            
        }

        private void SaveOrder()
        {
            VehicleOrder o = new VehicleOrder();
            o.Amount = CurrentView.Amount.Value;
            o.DateFrom = CurrentView.DateFrom.Value;
            o.DateTo = CurrentView.DateTo.Value;
            o.RefPerson = CurrentView.Contact.RefObjectId;
            //o.OrderType = CurrentView.OrderType.SelectedItem as EvdEnumValue;
            o.OrderType = GlauxSoft.GreenTransport.Repository.Enums.OrderType.Booking;
            o.Save();
            ViewModel.OrderId = o.ObjectID;
        }

        private void PrintEForm()
        {
            //eDocGenTemplateBuilder.General.
            var order = GlauxSoft.GreenTransport.Queries.QueryFactory.VehicleOrder.GetOrderById.GetObjects<VehicleOrder>(int.Parse(ViewModel.OrderId.ToString()));
            //var docCreator = new DocumentCreator(null, ViewModel.OrderId, null, "de", eDocGenCommon.Globals.DocCreatingUIMode.ShowDialog, @"D:\reports\report_" + ViewModel.OrderId, order);
            //docCreator.RootObjectID = ViewModel.OrderId;
            
            //var gen = new DocumentCreator();
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

        public ActionResult SearchPersonEvent()
        {
            InitGrid();
            return new DoNothingResult();
        }
      
        public ActionResult CreatePerson()
        {
            //NovaForm form = CurrentView.FrmRelationsWizard;
            //if (form != null)
            //{
            //    form.WizardGoNext();

            //    ViewModel.CurrentPageNumber = (byte)Pages.Create;
            //    UpdateControls();
            //}

            return new DoNothingResult();
        }

        public ActionResult GridRowChangedEvent()
        {
            return new DoNothingResult();
        }
        #endregion
    }
}
