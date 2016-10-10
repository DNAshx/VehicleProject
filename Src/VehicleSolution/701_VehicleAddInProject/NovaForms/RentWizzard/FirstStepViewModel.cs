using System;
using Evidence.Business;
using Evidence.Nova.Common;
using GreenTransport.ViewModels;

using GlauxSoft.GreenTransport.Repository;

namespace GreenTransport.NovaForms.RentWizzard
{
    public sealed class FirstStepViewModel : ViewModelBase
    {
        public FirstStepViewModel()
        {
            VehicleList = new ViewModelList<VehicleModelObject>();
            PersonList = new ViewModelList<PersonGridViewModel>();
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
    }
}
