using System;
using Evidence.Nova.Common;
using Evidence.Nova.Common.Extensibility;
using Vehicle.NovaForms.DemoView;

namespace Vehicle
{
    //if you do change name of this class or namespace you also have to change it inside nova.config
    public class VehicleAddIn : NovaAddinBase
    {
        protected override void Initialize()
        {
            base.RegisterAddInFunction(new AddinFunctionDescription
            {
                ID = new Guid("CFD2F0FF-DEEE-41FE-A277-5BAAB76A6868"),
                Text = "Demo Addin (VehicleAddIn)",
                Image = NovaResourceLinker.CreateLink("Vehicle.Images.demoaddin.svg"),
                TargetController = typeof(DemoViewController),
                TargetAction = "Index"
            });
        }
    }
}
