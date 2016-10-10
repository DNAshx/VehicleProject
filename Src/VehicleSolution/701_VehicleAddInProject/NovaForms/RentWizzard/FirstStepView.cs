using Evidence.Nova.Common;

namespace GreenTransport.NovaForms.RentWizzard
{
    public class FirstStepView : NovaViewBase
    {
        internal string OrderName
        {
            get { return FindElementByName<NovaText>().Text; }
            set { FindElementByName<NovaText>().Text = value; }
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

        internal NovaCombo VehicleType
        {
            get { return FindElementByName<NovaCombo>(); }
        }
        internal NovaCombo VehicleClass
        {
            get { return FindElementByName<NovaCombo>(); }
        }

        internal NovaGrid GridVehicles
        {
            get { return FindElementByName<NovaGrid>(); }
        }

        public NovaGrid GridPersons
        {
            get { return base.FindElementByName<NovaGrid>("gridPersons"); }
        }

        public NovaText SearchPersonField
        {
            get { return base.FindElementByName<NovaText>(); }
        }

        public NovaRef Order
        {
            get { return base.FindElementByName<NovaRef>(); }
        }

        internal NovaDateTime DateFrom
        {
            get { return FindElementByName<NovaDateTime>(); }
        }
        internal NovaDateTime DateTo
        {
            get { return FindElementByName<NovaDateTime>(); }
        }

        internal NovaNumeric Amount
        {
            get { return FindElementByName<NovaNumeric>(); }
        }

        internal NovaCombo OrderType
        {
            get { return FindElementByName<NovaCombo>(); }
        }
    }
}
