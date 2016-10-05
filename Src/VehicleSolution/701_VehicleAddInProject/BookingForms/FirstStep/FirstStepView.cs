using Evidence.Nova.Common;
namespace GreenTransport.BookingForms.FirstStep
{
    public class FirstStepView : NovaViewBase
    {
        //internal string OrderName
        //{
        //    get { return FindElementByName<NovaText>().Text; }
        //    set { FindElementByName<NovaText>().Text = value; }
        //}

		internal NovaImg Image
        {
            get { return FindElementByName<NovaImg>(); }
        }

        internal NovaRef Contact
        {
            get { return FindElementByName<NovaRef>(); }
        }

        internal NovaRef Vehicle
        {
            get { return FindElementByName<NovaRef>(); }
        }

        internal NovaDateTime StartDate
        {
            get { return FindElementByName<NovaDateTime>(); }
        }
        internal NovaDateTime EndDate
        {
            get { return FindElementByName<NovaDateTime>(); }
        }
        public NovaForm FrmFirstStepView
        {
            get { return base.FindElementByName<NovaForm>("frmGreenTransportStartView"); }
        }
    }
}
