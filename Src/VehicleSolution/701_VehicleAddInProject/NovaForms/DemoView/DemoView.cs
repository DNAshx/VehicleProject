using Evidence.Nova.Common;
namespace Vehicle.NovaForms.DemoView
{
    public class DemoView : NovaViewBase
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
        
    }
}
