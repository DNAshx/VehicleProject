using Evidence.Business;
using Evidence.Nova.Common;

namespace GreenTransport.BookingForms.FirstStep
{
    public sealed class FirstStepViewModel : Evidence.Nova.Common.ViewModelObject
    {
        public FirstStepViewModel()
        {
            VehicleList = new ViewModelList<VehicleModelObject>();
        }
        public EvidenceObject EvdObj
        {
            get { return (EvidenceObject)base.GetValue(); }
            set { base.SetValue(value); }
        }

        public string DialogResponseAction
        {
            get { return (string)base.GetValue(); }
            set { base.SetValue(value); }
        }

        public bool IsChanged
        {
            get { return (bool)base.GetValue(); }
            set { base.SetValue(value); }
        }

        public byte CurrentPageNumber
        {
            get { return (byte)GetValue(); }
            set { SetValue(value); }
        }

        public ViewModelList<VehicleModelObject> VehicleList
        {
            get { return (ViewModelList<VehicleModelObject>)GetValue(); }
            private set { SetValue(value); }
        }

    }
}
