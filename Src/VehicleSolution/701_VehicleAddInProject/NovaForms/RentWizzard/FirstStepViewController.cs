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
        static List<EvdEnumValue> listVehicleTypes = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.VehicleType.Bicycle, GlauxSoft.GreenTransport.Repository.Enums.VehicleType.Car });
        static List<EvdEnumValue> listCarType = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.CarType.Petrol, GlauxSoft.GreenTransport.Repository.Enums.CarType.Diesel, GlauxSoft.GreenTransport.Repository.Enums.CarType.Hybrid, GlauxSoft.GreenTransport.Repository.Enums.CarType.Electric});
        static List<EvdEnumValue> listBycicleType = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.BicycleType.Electric, GlauxSoft.GreenTransport.Repository.Enums.BicycleType.Ordinary});
        static List<EvdEnumValue> listCarClasses = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.CarClass.Small, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Medium, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Large, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Estate, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Premium, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Carriers, GlauxSoft.GreenTransport.Repository.Enums.CarClass.SUV });

        static string[] listBycicleTypeNames = new string[] { "Electric","Ordinary"};
        static string[] listCarTypeNames = new string[] { "Petrol","Diesel","Hybrid","Electric"};

        public FirstStepView CurrentView
        {
            get { return GetView<FirstStepView>(); }
        }

        public ActionResult Index()
        {
            CurrentView.Pick.RefClassId = BusinessDirectory.get_ClassDescriptor(myConst.Location.CLASSNAME).ID;
            CurrentView.Drop.RefClassId = BusinessDirectory.get_ClassDescriptor(myConst.Location.CLASSNAME).ID;

            CurrentView.Contact.RefClassId = BusinessDirectory.get_ClassDescriptor(myConst.Person.CLASSNAME).ID;
            CurrentView.Order.RefClassId = BusinessDirectory.get_ClassDescriptor(myConst.VehicleOrder.CLASSNAME).ID;
            CurrentView.PersonRef.RefClassId = BusinessDirectory.get_ClassDescriptor(myConst.Person.CLASSNAME).ID;

            CurrentView.GridVehicles.ItemsSource = ViewModel.VehicleList;
            CurrentView.GridPersons.ItemsSource = ViewModel.PersonList;
            CurrentView.GridSelectedVehicles.ItemsSource = ViewModel.VehicleSelectedList;

            CurrentView.StartDate.Value = DateTime.Today;
            CurrentView.EndDate.Value = DateTime.Today.AddDays(1);

            InitCombobox(CurrentView.VehicleType, listVehicleTypes);
            InitCombobox(CurrentView.BycicleType, listBycicleType);
            InitCombobox(CurrentView.CarType, listCarType);
            InitCombobox(CurrentView.CarClass, listCarClasses);

            var form = CurrentView.Root as NovaForm;
            if (form == null) throw new NovaException("Wrong type " + form.GetType());
            form.Title = "Booking process.";
            var actCnt = CurrentView.GridVehicles.Actions.Count;
            for (int ind = 1; ind < actCnt; ++ind)
            {
                var actn = CurrentView.GridVehicles.Actions.FindActionByName(string.Format("A{0}", ind));
                actn.IsEnabled = false;
                actn.Caption = string.Empty;
            }
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
            if (ViewModel.CurrentStep == FirstStepViewModel.WizzardSteps.Vehicle && !ViewModel.VehicleList.Any(v => v.Selected))
            {
                return new NovaMessageBoxDialog()
                {
                    Text = "You should select at least 1 vehicle",
                    Title = "Select Vehicle!",
                    Buttons = NovaMessageBoxButtons.Ok,
                    DefaultButton = NovaMessageBoxButtonDefault.No,
                    Icon = NovaMessageBoxIcon.Warning
                };
            }
            if (ViewModel.CurrentStep == FirstStepViewModel.WizzardSteps.Person && ViewModel.PersonList.Count(v => v.ToSelect) != 1)
            {
                return new NovaMessageBoxDialog()
                {
                    Text = "You should select 1 contact",
                    Title = "Select Contact!",
                    Buttons = NovaMessageBoxButtons.Ok,
                    DefaultButton = NovaMessageBoxButtonDefault.No,
                    Icon = NovaMessageBoxIcon.Warning
                };
            }
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
            CurrentView.OrderName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            CurrentView.DateFrom.Value = CurrentView.StartDate.Value;
            CurrentView.DateTo.Value = CurrentView.EndDate.Value;

            ViewModel.VehicleSelectedList.Clear();
            
            foreach (var v in ViewModel.VehicleList.Where(v => v.Selected))
            {
                ViewModel.VehicleSelectedList.Add(v);
            }

            var amount = ViewModel.VehicleList.Where(v => v.Selected).Sum(v => (decimal)v.PriceDay);
            var days = (int)(CurrentView.DateTo.Value - CurrentView.DateFrom.Value).Value.TotalDays;
            amount *= days;
            CurrentView.Amount.Value = amount;

            var person = ViewModel.PersonList.FirstOrDefault(p => p.ToSelect);
            CurrentView.PersonRef.RefObjectId = person.PersonID;
            CurrentView.PersonRef.Value = string.Format("{0} {1}", person.LastName, person.FirstName);
                        
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
            o.RefVehicle = ViewModel.VehicleSelectedList.First().VehicleId;    
            switch (CurrentView.OrderType.SelectedItem.ItemId)
            {
                case 0:
                    o.OrderType = GlauxSoft.GreenTransport.Repository.Enums.OrderType.Booking;
                    break;
                case 1:
                    o.OrderType = GlauxSoft.GreenTransport.Repository.Enums.OrderType.Holding;
                    break;
                case 2:
                    o.OrderType = GlauxSoft.GreenTransport.Repository.Enums.OrderType.Service;
                    break;
            }                        
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
            var selVhType = CurrentView.VehicleType.SelectedItem;
            
            var isBycicle = (selVhType!=null)?(selVhType.ItemId == CurrentView.VehicleType.Items[2].ItemId):(false);
            var isCar = (selVhType!=null)?(selVhType.ItemId == CurrentView.VehicleType.Items[1].ItemId):(false);
            // bycicle settings
            CurrentView.BycicleType.Visibility = (isBycicle) ? (Visibility.Visible) : (Visibility.Hidden);
            // car options
            CurrentView.CarType.Visibility = (isCar) ? (Visibility.Visible) : (Visibility.Hidden);
            CurrentView.CarClass.Visibility = (isCar) ? (Visibility.Visible) : (Visibility.Hidden);
            //
            var actCnt = CurrentView.GridVehicles.Actions.Count;
            for (int ind = 1; ind < actCnt; ++ind)
            {
                var actn = CurrentView.GridVehicles.Actions.FindActionByName(string.Format("A{0}",ind));
                actn.Caption = string.Empty;
                actn.IsEnabled = false;
                var actInd = ind - 1;
                if(isBycicle)
                {
                    actn.IsEnabled = (actInd < listBycicleTypeNames.Length);
                    if(actn.IsEnabled)
                    {
                        actn.Caption = listBycicleTypeNames[actInd];
                    }
                }
                if (isCar)
                {
                    actn.IsEnabled = (actInd < listCarTypeNames.Length);
                    if (actn.IsEnabled)
                    {
                        actn.Caption = listCarTypeNames[actInd];
                    }
                }
            }
            
            InitGrid();
            return new DoNothingResult();
        }

        public ActionResult SelectContact()
        {
            CurrentView.Contact.ActionAfterSelectedObjectChanged = "SelectContact";

            return new DoNothingResult();
        }

        public ActionResult SearchPersonEvent()
        {
            InitGrid();
            return new DoNothingResult();
        }
      
        public ActionResult CreatePerson()
        {
            
            return new DoNothingResult();
        }

        public ActionResult GridRowChangedEvent()
        {
            //
            return new DoNothingResult();
        }
        #endregion
    }
}
