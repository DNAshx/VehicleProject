using System;
using System.Collections.Generic;
using System.Linq;
using Evidence.Business;
using Evidence.Nova.Common;
using GlauxSoft.Business;
using GreenTransport.ViewModels;

namespace GreenTransport.Controllers
{
    public class BaseController<T> : ControllerBase<T> where T : ViewModelObject, new()
    {
        protected const int PLEASE_SELECT_ID = -1; // 0 means no query parameter
        private readonly string PLEASE_SELECT_MESSAGE = "Please select";//CMResources.Strings.PleaseSelectText;

        

        #region Combobox

        protected NovaCombo InitCombobox(NovaCombo combo, EvidenceEnum evidenceEnum, EvidenceAttribute enumAttribute = null)
        {
            combo.Items.Clear();

            foreach (EvdEnumValue p in evidenceEnum.EnumValues.OfType<EvdEnumValue>().OrderBy(x => x.OrderID))
                combo.Items.Add(new NovaComboItem { ItemId = p.ID.GetValue(), Caption = p.Caption });

            EvdEnumValId defaultValue = enumAttribute != null && enumAttribute.DefaultValue != null
                    ? (EvdEnumValId)enumAttribute.DefaultValue
                    : EvdEnumValId.Empty;
            bool required = enumAttribute != null && enumAttribute.Required;

            if (defaultValue != EvdEnumValId.Empty)
            {
                foreach (var item in combo.Items)
                    if (item.ItemId == defaultValue.GetValue())
                    {
                        combo.SelectedItem = item;
                        break;
                    }
            }

            if ((!required && defaultValue == EvdEnumValId.Empty) || required)
            {
                combo.Items.Insert(0, new NovaComboItem { ItemId = PLEASE_SELECT_ID, Caption = PLEASE_SELECT_MESSAGE });
                combo.SelectedItem = combo.Items[0];
            }

            if (combo.SelectedItem == null && combo.Items.Count > 0)
                combo.SelectedItem = combo.Items[0];

            return combo;
        }

        protected NovaCombo InitCombobox(NovaCombo combo, List<EvdEnumValue> evidenceEnumValues, EvidenceAttribute enumAttribute = null)
        {
            foreach (EvdEnumValue p in evidenceEnumValues.OrderBy(x => x.OrderID))
                combo.Items.Add(new NovaComboItem { ItemId = p.ID.GetValue(), Caption = p.Caption });

            EvdEnumValId defaultValue = enumAttribute != null && enumAttribute.DefaultValue != null
                    ? (EvdEnumValId)enumAttribute.DefaultValue
                    : EvdEnumValId.Empty;
            bool required = enumAttribute != null && enumAttribute.Required;

            if (defaultValue != EvdEnumValId.Empty)
            {
                foreach (var item in combo.Items)
                    if (item.ItemId == defaultValue.GetValue())
                    {
                        combo.SelectedItem = item;
                        break;
                    }
            }

            if (!required && defaultValue == EvdEnumValId.Empty)
            {
                combo.Items.Insert(0, new NovaComboItem { ItemId = PLEASE_SELECT_ID, Caption = "" });//PLEASE_SELECT_MESSAGE });
                combo.SelectedItem = combo.Items[0];
            }

            if (combo.SelectedItem == null && combo.Items.Count > 0)
                combo.SelectedItem = combo.Items[0];

            return combo;
        }

        protected NovaCombo InitCombobox(NovaCombo combo, List<EvdEnumValue> evidenceEnumValues, int? defaultId)
        {
            NovaCombo combo1 = InitCombobox(combo, evidenceEnumValues);
            if (defaultId != null)
            {
                foreach (var item in combo1.Items)
                    if (item.ItemId == defaultId)
                    {
                        combo1.SelectedItem = item;
                        break;
                    }
            }

            return combo1;
        }

        protected NovaCombo InitRelationCombobox(NovaCombo combo, string IDs)
        {
            var relationIDs = IDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string rid in relationIDs)
            {
                int relID;
                int.TryParse(rid, out relID);
                if (relID <= 0)
                    continue;

                var relation = Agent.get_EvidenceRelation(new EvdRelationId(relID));
                combo.Items.Add(new NovaComboItem { ItemId = relation.ID.GetValue(), Caption = relation.RoleBCaption });
            }

            if (!combo.Items.Count.Equals(1))
                combo.Items.Insert(0, new NovaComboItem { ItemId = PLEASE_SELECT_ID, Caption = "" });

            combo.SelectedItem = combo.Items[0];

            return combo;
        }

        protected void FComboSelect(NovaCombo combo, EvdEnumValId enumValId)
        {
            foreach (var item in combo.Items)
                if (item.ItemId == enumValId.GetValue())
                {
                    combo.SelectedItem = item;
                    return;
                }

            combo.SelectedItem = combo.Items.FirstOrDefault(x => x.ItemId == PLEASE_SELECT_ID);
        }

        protected void FComboSelect(NovaCombo combo, string caption)
        {
            foreach (var item in combo.Items)
                if (item.Caption == caption)
                {
                    combo.SelectedItem = item;
                    return;
                }

            combo.SelectedItem = combo.Items.FirstOrDefault(x => x.ItemId == PLEASE_SELECT_ID);
        }

        protected NovaCombo InitCombobox(NovaCombo combo, Dictionary<int, string> comboEntries, int? defaultId = null)
        {
            combo.Items.Clear();

            foreach (int key in comboEntries.Keys)
                combo.Items.Add(new NovaComboItem { ItemId = key, Caption = comboEntries[key] });

            if (defaultId != null)
            {
                foreach (var item in combo.Items)
                    if (item.ItemId == defaultId)
                    {
                        combo.SelectedItem = item;
                        break;
                    }
            }

            if (combo.SelectedItem == null && combo.Items.Count > 0)
                combo.SelectedItem = combo.Items[0];

            return combo;
        }

        #endregion

        
        #region Expression Fields
        
        /// <summary>
        /// Workaround bis das nova kann
        /// Siehe https://issuemgmt.glauxsoft.ch/issue/NOVA-1049
        /// </summary>
        /// <param name="obj"></param>
        public void FillExpressionFields(EvidenceObject obj)
        {

            if (obj == null)
                return;

            if (BusinessDirectory == null)
                throw new ArgumentNullException("BusinessDirectory"); //sollte eigentlich nicht vorkommen


            EvidenceClass cls = BusinessDirectory.get_ClassDescriptor(obj.ClassID);

            Guid cacheIdentifier = Guid.NewGuid();

            foreach (EvidenceAttribute attr in cls.Attrs)
            {
                try
                {
                    if (attr.Type == AttributeType.XExpression)
                    {
                        if (attr.ExprType == AttributeType.XString)
                        {
                            NovaText novaText = (NovaText)View.FindElementByName(attr.Name);
                            if (novaText != null)
                            {
                                object value = GetAttrValueByPath(obj, attr.Expression, cacheIdentifier);
                                if (value != null)
                                {
                                    novaText.EvdSourceAttr = null;
                                    novaText.Label = attr.Caption;
                                    novaText.IsReadOnly = true;
                                    novaText.IsEnabled = true;
                                    novaText.Text = (string)value;
                                }
                                else
                                    novaText.Text = string.Empty;
                            }
                        }
                        else if (attr.ExprType == AttributeType.XMLString)
                        {
                            NovaMLText novaMLText = (NovaMLText)View.FindElementByName(attr.Name);
                            if (novaMLText != null)
                            {
                                object value = GetAttrValueByPath(obj, attr.Expression, cacheIdentifier);
                                if (value != null)
                                {
                                    novaMLText.EvdSourceAttr = null;
                                    novaMLText.Label = attr.Caption;
                                    novaMLText.IsReadOnly = true;
                                    novaMLText.IsEnabled = true;
                                    throw new Exception("Can't set field");
                                }
                            }
                        }
                        else if (attr.ExprType == AttributeType.XLongText)
                        {
                            NovaLongText novaLongText = (NovaLongText)View.FindElementByName(attr.Name);
                            if (novaLongText != null)
                            {
                                object value = GetAttrValueByPath(obj, attr.Expression, cacheIdentifier);
                                if (value != null)
                                {
                                    novaLongText.EvdSourceAttr = null;
                                    novaLongText.Label = attr.Caption;
                                    novaLongText.IsReadOnly = true;
                                    novaLongText.IsEnabled = true;
                                    novaLongText.Text = (string)value;
                                }
                                else
                                    novaLongText.Text = string.Empty;
                            }
                        }
                        else if (attr.ExprType == AttributeType.XEnum)
                        {
                            // We use here NovaText only, not NovaCombo
                            // Add manually if required
                            NovaText novaText = (NovaText)View.FindElementByName(attr.Name);
                            if (novaText != null)
                            {
                                object value = GetAttrValueByPath(obj, attr.Expression, cacheIdentifier);
                                if (value != null)
                                {
                                    var enumValue = BusinessDirectory.get_EvdEnumValue((EvdEnumValId)value);

                                    novaText.EvdSourceAttr = null;
                                    novaText.Label = attr.Caption;
                                    novaText.IsReadOnly = true;
                                    novaText.IsEnabled = true;
                                    novaText.Text = enumValue.Caption;
                                }
                                else
                                    novaText.Text = string.Empty;
                            }
                        }
                        else if (attr.ExprType == AttributeType.XPhone)
                        {
                            NovaPhone novaPhone = (NovaPhone)View.FindElementByName(attr.Name);
                            if (novaPhone != null)
                            {
                                object value = GetAttrValueByPath(obj, attr.Expression, cacheIdentifier);
                                if (value != null)
                                {
                                    novaPhone.EvdSourceAttr = null;
                                    novaPhone.Label = attr.Caption;
                                    novaPhone.IsReadOnly = true;
                                    novaPhone.IsEnabled = true;
                                    novaPhone.Value = (string)value;
                                }
                                else
                                    novaPhone.Value = string.Empty;
                            }
                        }
                        else if (attr.ExprType == AttributeType.XFax)
                        {
                            NovaFax novaFax = (NovaFax)View.FindElementByName(attr.Name);
                            if (novaFax != null)
                            {
                                object value = GetAttrValueByPath(obj, attr.Expression, cacheIdentifier);
                                if (value != null)
                                {
                                    novaFax.EvdSourceAttr = null;
                                    novaFax.Label = attr.Caption;
                                    novaFax.IsReadOnly = true;
                                    novaFax.IsEnabled = true;
                                    novaFax.Value = (string)value;
                                }
                                else
                                    novaFax.Value = string.Empty;
                            }
                        }
                        else if (attr.ExprType == AttributeType.XEmail)
                        {
                            NovaMail novaMail = (NovaMail)View.FindElementByName(attr.Name);
                            if (novaMail != null)
                            {
                                object value = GetAttrValueByPath(obj, attr.Expression, cacheIdentifier);
                                if (value != null)
                                {
                                    novaMail.EvdSourceAttr = null;
                                    novaMail.Label = attr.Caption;
                                    novaMail.IsReadOnly = true;
                                    novaMail.IsEnabled = true;
                                    novaMail.Value = (string)value;
                                }
                                else
                                    novaMail.Value = string.Empty;
                            }
                        }
                        else if (attr.ExprType == AttributeType.XWebLink)
                        {
                            NovaWeb novaWeb = (NovaWeb)View.FindElementByName(attr.Name);
                            if (novaWeb != null)
                            {
                                object value = GetAttrValueByPath(obj, attr.Expression, cacheIdentifier);
                                if (value != null)
                                {
                                    novaWeb.EvdSourceAttr = null;
                                    novaWeb.Label = attr.Caption;
                                    novaWeb.IsReadOnly = true;
                                    novaWeb.IsEnabled = true;
                                    novaWeb.Value = (string)value;
                                }
                                else
                                    novaWeb.Value = string.Empty;
                            }
                        }
                        else if (attr.ExprType == AttributeType.XBoolean)
                        {
                            NovaCheckBox novaCheckBox = (NovaCheckBox)View.FindElementByName(attr.Name);
                            if (novaCheckBox != null)
                            {
                                object value = GetAttrValueByPath(obj, attr.Expression, cacheIdentifier);
                                if (value != null)
                                {
                                    novaCheckBox.EvdSourceAttr = null;
                                    novaCheckBox.Label = attr.Caption;
                                    novaCheckBox.IsEnabled = false;
                                    novaCheckBox.IsChecked = (bool)value;
                                }
                                else
                                    novaCheckBox.IsChecked = false;
                            }
                        }
                        else if (attr.ExprType == AttributeType.XInteger || attr.ExprType == AttributeType.XFloat || attr.ExprType == AttributeType.XDecimal)
                        {
                            NovaNumeric novaNumeric = (NovaNumeric)View.FindElementByName(attr.Name);
                            if (novaNumeric != null)
                            {
                                object value = GetAttrValueByPath(obj, attr.Expression, cacheIdentifier);
                                if (value != null)
                                {
                                    novaNumeric.EvdSourceAttr = null;
                                    novaNumeric.Label = attr.Caption;
                                    novaNumeric.IsReadOnly = true;
                                    novaNumeric.IsEnabled = true;
                                    novaNumeric.Value = ConvertToDecimal(value);
                                }
                                else
                                    novaNumeric.Value = null;
                            }
                        }
                        else if (attr.ExprType == AttributeType.XDataTime)
                        {
                            NovaDateTime novaDate = (NovaDateTime)View.FindElementByName(attr.Name);
                            if (novaDate != null)
                            {
                                object value = GetAttrValueByPath(obj, attr.Expression, cacheIdentifier);
                                if (value != null)
                                {
                                    switch (attr.Format)
                                    {
                                        case "t":
                                            novaDate.DateMode = NovaDateTimeMode.Time;
                                            break;
                                        case "d":
                                            novaDate.DateMode = NovaDateTimeMode.Date;
                                            break;
                                        default:
                                            novaDate.DateMode = NovaDateTimeMode.DateTime;
                                            break;
                                    }

                                    novaDate.EvdSourceAttr = null;
                                    novaDate.Label = attr.Caption;
                                    novaDate.IsReadOnly = true;
                                    novaDate.IsEnabled = true;
                                    novaDate.Value = (DateTime)value;
                                }
                                else
                                    novaDate.Value = null;
                            }
                        }
                    }
                }

                catch (Exception)
                {
                    //es darf unter keinen Umstaenden eine Exception geschossen werden
                }
            }
        }

        private decimal? ConvertToDecimal(object obj)
        {
            if (obj == null)
                return null;

            decimal? result = null;
            try
            {
                result = (decimal?)obj;
            }
            catch { /* this was not a decimal */}

            try
            {
                result = Convert.ToDecimal(obj);
            }
            catch { /* this was not a decimal */}

            return result;
        }

        private Dictionary<int, EvidenceObject> evdObjectCache;
        private Guid currentCacheIdentifier;

        private object GetAttrValueByPath(EvidenceObject eviObj, string path, Guid cacheIdentifier)
        {
            try
            {
                if (eviObj == null)
                    return null;

                if (!path.Contains("."))
                    return eviObj.get_AttributeValue(path);

                if (evdObjectCache == null || currentCacheIdentifier != cacheIdentifier)
                {
                    currentCacheIdentifier = cacheIdentifier;
                    evdObjectCache = new Dictionary<int, EvidenceObject>();
                }

                int index = path.IndexOf(".", StringComparison.Ordinal);
                string reference = path.Substring(0, index);
                object refObjectId = eviObj.get_AttributeValue(reference);
                if (refObjectId == null)
                    return null;

                EvidenceAttribute attr = SessionManager.Session.Directory.get_AttrDescriptor(eviObj.ClassName, reference);
                if (attr == null)
                    return null;

                EvidenceObject refObj = null;
                EvdObjectId objId = (EvdObjectId)refObjectId;
                if (evdObjectCache.ContainsKey(objId.GetValue()))
                    refObj = evdObjectCache[objId.GetValue()];
                else
                {
                    SessionManager.Session.Agent.GetObject(attr.RefClassID, objId, ref refObj);
                    if (refObj != null)
                        evdObjectCache.Add(objId.GetValue(), refObj);
                }

                if (refObj == null)
                    return null;

                return GetAttrValueByPath(refObj, path.Substring(index + 1), currentCacheIdentifier);
            }
            catch
            {
                return null;
            }
        }

        #endregion


        public ActionResult DoNothingActionMethod()
        {
            return new DoNothingResult();
        }
    }
}
