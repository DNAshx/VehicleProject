using Evidence.Business;
using Evidence.Nova.Common;
using Evidence.Services;

namespace GreenTransport.NovaForms.DefaultDetailForm
{
    [DefaultView(typeof(DefaultDetailFormView))]
    public class DefaultDetailFormController : ControllerBase<DefaultDetailFormViewModel>
    {
        public ActionComposer Index()
        {
            var composer = new ActionComposer();

            var dlg = base.GetArgument<NovaEvidenceObjectDialog>();

            if (dlg != null)
            {
                ViewModel.DialogResponseAction = dlg.DialogResponseAction;
                var cls = this.BusinessDirectory.get_ClassDescriptor(dlg.ClassID);
                //AN: TODO NovaEvidenceObjectDialog also has properties RequestSource, CandidateObjectId, CandidateObjectClass, InputOutputParameter which may need to be processed some way 
                if (cls != null)
                    InitView(cls, dlg.IsReadonly);
            }

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
            var view = GetView<DefaultDetailFormView>();

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
                        form.UIElementTree.Add(new NovaCheckBox {EvdSourceAttr = sourceAttr, IsEnabled = !viewOnly});
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
                        form.UIElementTree.Add(new NovaDoc { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly, HasDocPreview = true});
                        break;
                    case AttributeType.XWebLink:
                        form.UIElementTree.Add(new NovaWeb { EvdSourceAttr = sourceAttr, IsReadOnly = viewOnly });
                        break;
                }
            }
        }
    }
}

