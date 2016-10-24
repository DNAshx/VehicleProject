using GreenTransport.NovaForms.DefaultDetailForm;

namespace GreenTransport
{
    public class ProjectConfig : Evidence.Nova.Common.NovaProjectConfig
    {
        protected override void Initialize()
        {
            // mapping evidence classes to views
            // please have a look at https://confluence.glauxsoft.ch/display/N2/Open+the+registered+NovaForm+via+Evidence+class+mapping            

            this.SetClassImage("Person", "GreenTransport.Images.person.svg");
            this.SetClassImage("Company", "GreenTransport.Images.company.svg");   
            AddClassMapping("Person", typeof(DefaultDetailFormController));
            AddClassMapping("Company", typeof(DefaultDetailFormController));
            AddClassMapping("Vehicle", typeof(GreenTransport.NovaForms.VehicleDetailsForm.VehicleDetailsFormController));
            AddClassMapping("VehicleOrder", typeof(DefaultDetailFormController));
            AddClassMapping("Location", typeof(DefaultDetailFormController));
            AddClassMapping("FuelConsumption", typeof(DefaultDetailFormController));
        }
    }
}
