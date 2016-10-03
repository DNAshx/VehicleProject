using System.IO;
using Evidence.Nova.Common;

namespace Vehicle.NovaForms.DemoView
{
	[DefaultView(typeof(DemoView))]
    public class DemoViewController : ControllerBase<DemoViewModel>
    {
        public ActionResult Index()
        {
			var view = GetView<DemoView>();
            view.AddinName = "VehicleAddIn";
			using (var stream = this.GetType().Assembly.GetManifestResourceStream("Vehicle.Images.owls.jpg"))
            {
                if (stream != null)
                {
                    byte[] ba = new byte[stream.Length];
                    stream.Read(ba, 0, ba.Length);
                    view.Image.ImageContent.SetContent(ba, ".jpg");
                }
            }

            return new DoNothingResult();
        }

		public ActionResult FormClose()
        {
            return new CloseDialogResult(true, null);
        }
    }
}
