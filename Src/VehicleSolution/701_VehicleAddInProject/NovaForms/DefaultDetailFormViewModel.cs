using Evidence.Business;
using Evidence.Nova.Common;

namespace GreenTransport.NovaForms
{
    public sealed class DefaultDetailFormViewModel : ViewModelObject
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

