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
            view.AddinName = string.Format("Order No. {0}", new Random().Next(1, 100));
            Random houseNo = new Random();
            Random countryCode = new Random(1000);
            Person p = BusinessObject.Create<Person>();

            p.FirstName = RandomProvider.NextFirstName();
            p.Nachname = RandomProvider.NextLastName();
            p.CityName = RandomProvider.NextCity();
            p.TelefonPrivat = RandomProvider.NextPhoneNumber();
            p.Adresse = RandomProvider.NextStreet();
            p.CountryCode = countryCode.Next(4000).ToString();
            p.Hausnummer = houseNo.Next(1, 40).ToString();
            p.Save();
            view.Contact.RefClassId = p.ClassID;
            view.Contact.IsRequired = true;

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

            return new NovaMessageBoxDialog()
            {
                Text = "Hello Nova AddIn!",
                Title = "Hello Nova AddIn!",
                Buttons = NovaMessageBoxButtons.Ok,
                DefaultButton = NovaMessageBoxButtonDefault.No,
                Icon = NovaMessageBoxIcon.Info
            };
        }

        public ActionResult FormClose()
        {
            return new CloseDialogResult(true, null);
        }


        /// <summary>
        /// Call the FormSave-function to save the evidence object
        /// </summary>
        /// <returns></returns>
        public ActionComposer FormSave()
        {
            var composer = new ActionComposer();
            composer.Add(() =>
            {
                // do some before-save stuff here...
                return null;
            });
            composer.Add(() =>
            {
                // call the formsave from the base controller
                return composer.SubControllerAction(() => this.RuntimeFormSave());
            });
            composer.Add(() =>
            {
                // do some after-save stuff here...
                ViewModel.IsChanged = true;
                return null;
            });
            return composer;
        }

        private void InitView(EvidenceClass evdClass, bool viewOnly)
        {
            var view = GetView<FirstStepView>();
            view.AddinName = "GreenTransport AddIn";
            using (var stream = this.GetType().Assembly.GetManifestResourceStream("GreenTransport.Images.owls.jpg"))
            {
                if (stream != null)
                {
                    byte[] ba = new byte[stream.Length];
                    stream.Read(ba, 0, ba.Length);
                    view.Image.ImageContent.SetContent(ba, ".jpg");
                }
            }

            var form = view.Root as NovaForm;
            if (form == null) throw new NovaException("Wrong type " + view.Root.GetType());
            form.Title = evdClass.Caption + " details";

            //TODO already initialized, see bug NOVA-955 Runtime form Index method is called twice 
            if (form.UIElementTree.Count > 0) return;
            foreach (EvidenceAttribute attrib in evdClass.Attrs)
            {
                if (!attrib.VisibleToUser)
                    continue;
                var sourceAttr = "EvdObj." + attrib.Name;
                switch (attrib.ExprType)
                {
                    case AttributeType.XBoolean:
                        form.UIElementTree.Add(new NovaCheckBox { EvdSourceAttr = sourceAttr, IsEnabled = !viewOnly });
                        break;
                    case AttributeType.XDataTime:
                        form.UIElementTree.Add(new NovaDateTime { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly, });
                        break;
                    case AttributeType.XDecimal:
                    case AttributeType.XFloat:
                    case AttributeType.XInteger:
                        //NovaNumeric should set up its format according attribute type itself
                        form.UIElementTree.Add(new NovaNumeric { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly });
                        break;
                    case AttributeType.XEmail:
                        form.UIElementTree.Add(new NovaMail { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly });
                        break;
                    case AttributeType.XEnum:
                        form.UIElementTree.Add(new NovaCombo { EvdSourceAttr = sourceAttr, IsEnabled = viewOnly });
                        break;
                    //case AttributeType.XExpression: //Can't be there
                    case AttributeType.XFax:
                        form.UIElementTree.Add(new NovaFax { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly });
                        break;
                    case AttributeType.XImage:
                        form.UIElementTree.Add(new NovaImg { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly });
                        break;
                    case AttributeType.XLongBinary:
                        //AN TODO NOVA-549 how it should be shown?
                        break;
                    case AttributeType.XLongText:
                        form.UIElementTree.Add(new NovaLongText { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly });
                        break;
                    case AttributeType.XMLLongText:
                        form.UIElementTree.Add(new NovaMLLongText { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly, });
                        break;
                    case AttributeType.XMLString:
                        form.UIElementTree.Add(new NovaMLText { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly });
                        break;
                    case AttributeType.XPhone:
                        form.UIElementTree.Add(new NovaPhone { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly });
                        break;
                    case AttributeType.XReference:
                        //NovaRef requires attrib name.
                        form.UIElementTree.Add(new NovaRef { EvdSourceAttr = sourceAttr, Name = attrib.Name, IsReadOnly = viewOnly });
                        break;
                    case AttributeType.XString:
                        form.UIElementTree.Add(new NovaText { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly });
                        break;
                    case AttributeType.XVDocument:
                        form.UIElementTree.Add(new NovaDoc { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly, HasDocPreview = true });
                        break;
                    case AttributeType.XWebLink:
                        form.UIElementTree.Add(new NovaWeb { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly });
                        break;
                }
            }
        }
    }
}
