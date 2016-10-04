using GreenTransport.BookingForms.FirstStepView;

namespace GreenTransport
{
    public class ProjectConfig : Evidence.Nova.Common.NovaProjectConfig
    {
        protected override void Initialize()
        {
            // mapping evidence classes to views
			// please have a look at https://confluence.glauxsoft.ch/display/N2/Open+the+registered+NovaForm+via+Evidence+class+mapping

            // AddClassMapping("Person", typeof(PersonViewController));

	        this.SetClassImage("Person", "GreenTransport.Images.person.svg");
            this.SetClassImage("Company", "GreenTransport.Images.company.svg");            
        }
    }
}
