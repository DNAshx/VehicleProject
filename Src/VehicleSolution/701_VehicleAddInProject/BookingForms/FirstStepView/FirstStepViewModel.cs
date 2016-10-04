using Evidence.Business;

namespace GreenTransport.BookingForms.FirstStepView
{
    public sealed class FirstStepViewModel : Evidence.Nova.Common.ViewModelObject
    {
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
    }
}
