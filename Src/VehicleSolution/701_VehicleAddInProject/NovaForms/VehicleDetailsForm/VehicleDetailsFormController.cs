using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

using Evidence.Business;
using Evidence.Services;
using Evidence.Nova.Common;

using GlauxSoft.Common;
using GlauxSoft.GreenTransport.Repository;
using GlauxSoft.Business;
using GreenTransport.Controllers;

using eDocGenDocumentCreator;

using myConst = GlauxSoft.GreenTransport.Repository.Constants;

namespace GreenTransport.NovaForms.VehicleDetailsForm
{
    [DefaultView(typeof(VehicleDetailsFormView))]
    public class VehicleDetailsFormController : BaseController<VehicleDetailsFormModel>
    {
        public VehicleDetailsFormView CurrentView
        {
            get { return GetView<VehicleDetailsFormView>(); }
        }

        public ActionComposer Index()
        {
            CurrentView.RefLocation.RefClassId = BusinessDirectory.get_ClassDescriptor(myConst.Location.CLASSNAME).ID;

            var composer = new ActionComposer();

            var dlg = base.GetArgument<NovaEvidenceObjectDialog>();

            if (dlg != null)
            {
                //CurrentView.Model.Text = string.Format("{0}-{1}-{2}", dlg.ObjectID, dlg.CandidateObjectId, ViewModel.EvdObj);
                var vehicles = GlauxSoft.GreenTransport.Queries.QueryFactory.Vehicle.VehicleGetById.GetObjects<Vehicle>(dlg.ObjectID.GetValue());
                var vehicle = vehicles.FirstOrDefault();
                if(vehicle!=null)
                {
                    //CurrentView.VehicleType.SelectedItem;
                    //CurrentView.Brand.
                    CurrentView.Model.Text = vehicle.Model;
                    CurrentView.RefLocation.RefObjectId = vehicle.RefLocation;
                    //CurrentView.VehicleClass.;
                    //CurrentView.co2;
                    //CurrentView.Description
                    CurrentView.Efficiency.Value = 1;// vehicle.Efficiency;
                    //CurrentView.Engine;
                    CurrentView.QtPassengers.Value = vehicle.QtPassengers;
                    CurrentView.PriceDay.Value = (decimal)vehicle.PriceDay.GetValueOrDefault();
                    CurrentView.ServiceHours.Value = vehicle.ServiceHours;
                    CurrentView.ProdDate.Value = vehicle.ProdDate.GetValueOrDefault();
                }
                //

                //
                ViewModel.DialogResponseAction = dlg.DialogResponseAction;
                var cls = this.BusinessDirectory.get_ClassDescriptor(dlg.ClassID);
                //AN: TODO NovaEvidenceObjectDialog also has properties RequestSource, CandidateObjectId, CandidateObjectClass, InputOutputParameter which may need to be processed some way 
                if (cls != null)
                    InitView(cls, dlg.IsReadonly);
            }

            var listTypes = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.VehicleType.Bicycle, GlauxSoft.GreenTransport.Repository.Enums.VehicleType.Car});
            var listCarTypes = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.CarType.Electric, GlauxSoft.GreenTransport.Repository.Enums.CarType.Diesel, GlauxSoft.GreenTransport.Repository.Enums.CarType.Hybrid, GlauxSoft.GreenTransport.Repository.Enums.CarType.Petrol });
            var listClasses = new List<EvdEnumValue>(new EvdEnumValue[] { GlauxSoft.GreenTransport.Repository.Enums.CarClass.Small, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Medium, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Large, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Estate, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Premium, GlauxSoft.GreenTransport.Repository.Enums.CarClass.Carriers, GlauxSoft.GreenTransport.Repository.Enums.CarClass.SUV });

            //InitCombobox(CurrentView.VehicleType, listTypes);
            InitCombobox(CurrentView.VehicleClass, listClasses);
            InitCombobox(CurrentView.VehicleClass, listClasses);
            //InitCombobox(CurrentView.Brand, listClasses);
            var d = this.ViewModel.EvdObj;

            //CurrentView.Model.Text = string.Format("");
            //CurrentView.VehicleType = 
            composer.Add(() =>
            {
                // Call the index-function of the base class
                return composer.SubControllerAction(() => this.RuntimeFormIndex());
            });
            composer.Add(() =>
            {
                // do some after loading stuff...
                return null;
            });
            composer.Finally(() =>
            {
                // clean up...
            });

            return composer;
        }

        public ActionComposer FormClose()
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
                return composer.SubControllerAction(() => this.RuntimeFormClose());
            });
            composer.Add(() =>
            {
                DialogAction ret = new NovaEvidenceObjectDialog(ViewModel.EvdObj.ClassID, ViewModel.EvdObj.ObjectID);
                ret.DialogResponseAction = ViewModel.DialogResponseAction;
                return new CloseDialogResult(ViewModel.IsChanged, ret);
            });
            return composer;
        }

        public ActionResult SearchEvent()
        {
            //InitGrid();
            return new DoNothingResult();
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
            var view = GetView<VehicleDetailsFormView>();

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