using System;
using Evidence.Business;
using Evidence.Nova.Common;
using GreenTransport.ViewModels;

using GlauxSoft.GreenTransport.Repository;
using System.Linq;
using System.Collections.Generic;

namespace GreenTransport.NovaForms.RentWizzard
{
    public sealed class FirstStepViewModel : ViewModelBase
    {
        public enum WizzardSteps
        {
            Vehicle,
            Person,
            Order,
            Finish
        }        

        public WizzardSteps CurrentStep
        {
            get { return (WizzardSteps)CurrentPageNumber; }            
        }
        public FirstStepViewModel()
        {
            VehicleList = new ViewModelList<VehicleModelObject>();
            PersonList = new ViewModelList<PersonGridViewModel>();
            VehicleSelectedList = new ViewModelList<VehicleModelObject>();
        }

        public byte CurrentPageNumber
        {
            get { return (byte)GetValue(); }
            set { SetValue(value); }
        }

        public DateTime StartDate { get { return (DateTime)GetValue(); } set { SetValue(value); } }
        public DateTime EndDate { get { return (DateTime)GetValue(); } set { SetValue(value); } }

        public ViewModelList<VehicleModelObject> VehicleList
        {
            get { return (ViewModelList<VehicleModelObject>)GetValue(); }
            private set { SetValue(value); }
        }
        public ViewModelList<VehicleModelObject> VehicleSelectedList
        {
            get { return (ViewModelList<VehicleModelObject>)GetValue(); }
            private set { SetValue(value); }
        }

        public int PersonID
        {
            get { return (int)GetValue(); }
            set { SetValue(value); }
        }

        public ViewModelList<PersonGridViewModel> PersonList
        {
            get { return (ViewModelList<PersonGridViewModel>)GetValue(); }
            set { SetValue(value); }
        }
        public EvdObjectId OrderId
        {
            get { return (EvdObjectId)GetValue(); }
            set { SetValue(value); }
        }

    }
}
