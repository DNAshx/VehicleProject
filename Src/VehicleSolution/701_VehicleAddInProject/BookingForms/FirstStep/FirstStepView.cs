using Evidence.Nova.Common;
namespace GreenTransport.BookingForms.FirstStep
{
    public class FirstStepView : NovaViewBase
    {
        internal string AddinName
        {
            get { return FindElementByName<NovaText>().Text; }
            set { FindElementByName<NovaText>().Text = value; }
        }

		internal NovaImg Image
        {
            get { return FindElementByName<NovaImg>(); }
        }
        public NovaForm FrmDefaultDetailForm
        {
            get { return base.FindElementByName<NovaForm>("frmGreenTransportStartView"); }
        }
    }
}
