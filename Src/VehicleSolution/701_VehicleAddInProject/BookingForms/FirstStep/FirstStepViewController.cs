using System.IO;
using Evidence.Nova.Common;
using Evidence.Business;
using System;
using GlauxSoft.Common;
using GlauxSoft.GreenTransport.Repository;
using GlauxSoft.Business;

namespace GreenTransport.BookingForms.FirstStep
{
	[DefaultView(typeof(FirstStepView))]
    public class FirstStepViewController : ControllerBase<FirstStepViewModel>
    {        
        public ActionResult Index()
        {
            var view = GetView<FirstStepView>();
            var form = view.Root as NovaForm;
            //view.OrderName = string.Format("Order No. {0}", new Random().Next(1, 100));
            view.Contact.RefClassId = ((BusinessObject)(new Person())).ClassID;
            view.Vehicle.RefClassId = ((BusinessObject)(new Vehicle())).ClassID;
            #region ///add contacts
            //Random houseNo = new Random();
            //Random countryCode = new Random(1000);
            //Person p = BusinessObject.Create<Person>();

            //p.FirstName = RandomProvider.NextFirstName();
            //p.Nachname = RandomProvider.NextLastName();
            //p.CityName = RandomProvider.NextCity();
            //p.TelefonPrivat = RandomProvider.NextPhoneNumber();
            //p.Adresse = RandomProvider.NextStreet();
            //p.CountryCode = countryCode.Next(4000).ToString();
            //p.Hausnummer = houseNo.Next(1, 40).ToString();
            //p.Save();
            //view.Contact.RefClassId = p.ClassID;
            //view.Contact.IsRequired = true;
            #endregion

            view.StartDate.Value = DateTime.Today;
            view.EndDate.Value = DateTime.Today.AddDays(1);
            using (var stream = this.GetType().Assembly.GetManifestResourceStream("GreenTransport.Images.owls.jpg"))
            {
                if (stream != null)
                {
                    byte[] ba = new byte[stream.Length];
                    stream.Read(ba, 0, ba.Length);
                    view.Image.ImageContent.SetContent(ba, ".jpg");
                }
            }

            
            if (form == null) throw new NovaException("Wrong type " + view.Root.GetType());
            form.Title = "Booking process.";

            return new DoNothingResult();//NovaMessageBoxDialog()
            //{
            //    Text = "Hello Nova AddIn!",
            //    Title = "Hello Nova AddIn!",
            //    Buttons = NovaMessageBoxButtons.Ok,
            //    DefaultButton = NovaMessageBoxButtonDefault.No,
            //    Icon = NovaMessageBoxIcon.Info
            //};
        }


        public ActionResult FormClose()
        {
            return new CloseDialogResult(true, null);
        }

        public ActionResult FrmWizardNext()
        {
            var form = View.FindElementByName<NovaForm>("frmGreenTransportStartView");
            if (form != null && form.WizardCanGoNext())
            {
                form.WizardGoNext();
                ViewModel.CurrentPageNumber++;                
            }
            return new DoNothingResult();
        }

        public ActionResult FrmWizardFinish()
        {
            //var order = BusinessObject.Create<VehicleOrder>();
            //order.Amount = 
            return new NovaMessageBoxDialog()
                {
                    Text = "Bucking order created successeful!",
                    Title = "Success!",
                    Buttons = NovaMessageBoxButtons.Ok,
                    DefaultButton = NovaMessageBoxButtonDefault.No,
                    Icon = NovaMessageBoxIcon.Info
                };
        }
        /// <summary>
        /// Call the FormSave-function to save the evidence object
        /// </summary>
        /// <returns></returns>
        public ActionResult FormSave()
        {            
            return new DoNothingResult();
        }
              
    }
}
