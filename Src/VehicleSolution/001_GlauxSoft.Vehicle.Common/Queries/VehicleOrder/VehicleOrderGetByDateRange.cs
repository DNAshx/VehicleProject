using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using Evidence.Business;
using Evidence.Services;
using GlauxSoft.Business;
using System.Collections;
using myConst = GlauxSoft.GreenTransport.Repository.Constants;

namespace GlauxSoft.GreenTransport.Queries
{
    /// <summary>Class for query definition and access to query called VehicleOrderGetByDateRange in evidence class VehicleOrder</summary>
    /// <remarks>Query names are unique over all evidence classes.</remarks>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("QueryBuilder", "1.35.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.Diagnostics.DebuggerDisplay("ClassName={ClassName}, QueryName=VehicleOrderGetByDateRange")]
    [EvidenceQuery(UseRepoConstants = true)]
    public class VehicleOrderGetByDateRange : QueryBase
    {
        //###########################################################################
        //# THIS CODE WAS AUTOGENERATED BY THE QUERYBUILDER TOOL.                   # 
        //# DO NOT CHANGE CODE MANUALLY. INSTEAD MAKE CHANGES THROUGH QUERYBUILDER. #
        //###########################################################################

        #region Properties


        /// <summary>
        /// Gets the class name of the complex query's root class.
        /// </summary>
        public override string ClassName
        {
            get
            {
                return myConst.VehicleOrder.CLASSNAME;
            }
        }

        /// <summary>
        /// Gets the comment that is attached to the complex query.
        /// </summary>
        public override string Comment
        {
            get
            {
                return "";
            }
        }

        #endregion //Properties

        #region Constructor

        // create an instance of this class via the QueryFactory
        internal VehicleOrderGetByDateRange(QueryFactoryContext context)
            : base(context)
        {
        }

        #endregion //Constructor

        #region Methods

        /// <summary>
        /// Builds the native complex query as it is defined in the evidence repository.
        /// </summary>
        /// <returns>A new ComplexQuery instance.</returns>
        /// <exception cref="InvalidQueryException">occurs if the query is not valid.</exception>
        protected override ComplexQuery LoadQueryInternal()
        {
            // Checks all classes/attributes/enums/relations which are in use in this query.
            // If one item is missing, it will raise an InvalidQueryException with message: Invalid query: non-existing or forbidden items are used.
            // It will not provide the missing items in the exception text, because of sequrity reasons. 
            // The list of missing items will be written into the log.
            base.VisitClass(myConst.VehicleOrder.CLASSNAME);
            base.VisitAttribute(myConst.VehicleOrder.CLASSNAME, myConst.VehicleOrder.ATTR_DATEFROM);
            base.VisitAttribute(myConst.VehicleOrder.CLASSNAME, myConst.VehicleOrder.ATTR_DATETO);

            base.CheckVisitedItemsAndThrowOnError();

            // create query tree

            MLString prmBee81e81b7a549108018609c4a6cba40Caption = new MLString(new EvdLangId(7), "prmDateFr", new EvdLangId(7));
            EvdParameter prmBee81e81b7a549108018609c4a6cba40 = new EvdParameter("prm_bee81e81b7a549108018609c4a6cba40", ParameterType.DateTime);
            prmBee81e81b7a549108018609c4a6cba40.Caption = prmBee81e81b7a549108018609c4a6cba40Caption;
            prmBee81e81b7a549108018609c4a6cba40.RowCount = 1;

            MLString prmBc9e3edc74fc449b977097a998c720bdCaption = new MLString(new EvdLangId(7), "prmDateTo", new EvdLangId(7));
            EvdParameter prmBc9e3edc74fc449b977097a998c720bd = new EvdParameter("prm_bc9e3edc74fc449b977097a998c720bd", ParameterType.DateTime);
            prmBc9e3edc74fc449b977097a998c720bd.Caption = prmBc9e3edc74fc449b977097a998c720bdCaption;
            prmBc9e3edc74fc449b977097a998c720bd.RowCount = 1;

            AttributeQuery query1 = AttributeQuery.Create(Directory.get_ClassDescriptor(myConst.VehicleOrder.CLASSNAME).ID);
            query1.AddRow(AttributeQueryRow.Create(Directory.get_AttrDescriptor(myConst.VehicleOrder.CLASSNAME, myConst.VehicleOrder.ATTR_DATEFROM), ComparisonOperators.GreaterOrEqualTo, prmBee81e81b7a549108018609c4a6cba40, LogicalConnector.LC_AND, null, Directory.get_ClassDescriptor(myConst.VehicleOrder.CLASSNAME).ID.GetValue()));
            query1.AddRow(AttributeQueryRow.Create(Directory.get_AttrDescriptor(myConst.VehicleOrder.CLASSNAME, myConst.VehicleOrder.ATTR_DATEFROM), ComparisonOperators.LessThen, prmBc9e3edc74fc449b977097a998c720bd, LogicalConnector.LC_AND, null, Directory.get_ClassDescriptor(myConst.VehicleOrder.CLASSNAME).ID.GetValue()));
            query1.AddRow(AttributeQueryRow.Create(Directory.get_AttrDescriptor(myConst.VehicleOrder.CLASSNAME, myConst.VehicleOrder.ATTR_DATETO), ComparisonOperators.GreaterThen, prmBee81e81b7a549108018609c4a6cba40, LogicalConnector.LC_OR, null, Directory.get_ClassDescriptor(myConst.VehicleOrder.CLASSNAME).ID.GetValue()));
            query1.AddRow(AttributeQueryRow.Create(Directory.get_AttrDescriptor(myConst.VehicleOrder.CLASSNAME, myConst.VehicleOrder.ATTR_DATETO), ComparisonOperators.LessOrEqualTo, prmBc9e3edc74fc449b977097a998c720bd, LogicalConnector.LC_AND, null, Directory.get_ClassDescriptor(myConst.VehicleOrder.CLASSNAME).ID.GetValue()));
            query1.Bracket(0, 0);
            query1.Bracket(1, 1);
            query1.Bracket(2, 2);

            ComplexQuery query0 = ComplexQuery.Create(query1);
            query0.Advanced = true;

            query0.Parameters.Add(prmBee81e81b7a549108018609c4a6cba40);
            query0.Parameters.Add(prmBc9e3edc74fc449b977097a998c720bd);

            query0.GetRootNodeQuery().ParamAskingType = ParameterAskingType.AskForAll;

            return query0;

        }


        /// <summary>
        /// Returns a prepared complex query ready for the execution. Any parameters is set and checked.
        /// If a parameter is of wrong type, missing or not provided by method input it will throw an exception.
        /// This behavior is the same as already implemented in core business object.
        /// </summary>
        public ComplexQuery GetComplexQuery(DateTime prmBee81e81b7a549108018609c4a6cba40, DateTime prmBc9e3edc74fc449b977097a998c720bd)
        {
            List<ParameterValue> parameters = new List<ParameterValue>();
            parameters.Add(new ParameterValue.DateTimeParameter("prm_bee81e81b7a549108018609c4a6cba40", prmBee81e81b7a549108018609c4a6cba40));
            parameters.Add(new ParameterValue.DateTimeParameter("prm_bc9e3edc74fc449b977097a998c720bd", prmBc9e3edc74fc449b977097a998c720bd));

            //return the result of the dynamic way
            return base.GetComplexQuery(parameters.ToArray());
        }

        /// <summary>
        /// Executes query "VehicleOrderGetByDateRange" and returns result as list of business objects. 
        /// Type of business object is the type which is configured in business layer config for the 
        /// provided class id of this query.
        /// </summary>
        public List<BusinessObject> GetObjects(DateTime prmBee81e81b7a549108018609c4a6cba40, DateTime prmBc9e3edc74fc449b977097a998c720bd)
        {
            List<ParameterValue> parameters = new List<ParameterValue>();
            parameters.Add(new ParameterValue.DateTimeParameter("prm_bee81e81b7a549108018609c4a6cba40", prmBee81e81b7a549108018609c4a6cba40));
            parameters.Add(new ParameterValue.DateTimeParameter("prm_bc9e3edc74fc449b977097a998c720bd", prmBc9e3edc74fc449b977097a998c720bd));

            return base.GetObjects(parameters.ToArray());

        }

        /// <summary>
        /// Executes query "VehicleOrderGetByDateRange" and returns result as list object business objects of type T.
        /// This method can be used with LightWeigthBusinessObjects too.
        /// </summary>
        public List<T> GetObjects<T>(DateTime prmBee81e81b7a549108018609c4a6cba40, DateTime prmBc9e3edc74fc449b977097a998c720bd)
            where T : CoreBusinessObject
        {
            List<ParameterValue> parameters = new List<ParameterValue>();
            parameters.Add(new ParameterValue.DateTimeParameter("prm_bee81e81b7a549108018609c4a6cba40", prmBee81e81b7a549108018609c4a6cba40));
            parameters.Add(new ParameterValue.DateTimeParameter("prm_bc9e3edc74fc449b977097a998c720bd", prmBc9e3edc74fc449b977097a998c720bd));

            //return the result of the dynamic way
            return base.GetObjects<T>(parameters.ToArray());
        }

        /// <summary>
        /// Returns a queryable instance for query "VehicleOrderGetByDateRange". See <see cref="CoreBusinessObject.AsQueryable"/> for queryable usage.
        /// </summary>
        /// <typeparam name="BusinessObject">The business object type to get evidence class information and mapping result data to.</typeparam>
        public BusinessObjectQueryable<BusinessObject> AsQueryable<BusinessObject>(DateTime prmBee81e81b7a549108018609c4a6cba40, DateTime prmBc9e3edc74fc449b977097a998c720bd)
            where BusinessObject : CoreBusinessObject
        {
            List<ParameterValue> parameters = new List<ParameterValue>();
            parameters.Add(new ParameterValue.DateTimeParameter("prm_bee81e81b7a549108018609c4a6cba40", prmBee81e81b7a549108018609c4a6cba40));
            parameters.Add(new ParameterValue.DateTimeParameter("prm_bc9e3edc74fc449b977097a998c720bd", prmBc9e3edc74fc449b977097a998c720bd));
            return (BusinessObjectQueryable<BusinessObject>)(AsQueryable<BusinessObject>(parameters.ToArray()));
        }

        #endregion // Methods

    }


    #region QueryAccessor

    // static typing to support intellisense programming.
    public partial class VehicleOrderQueryAccessor : QueryAccessorBase
    {
        /// <summary>
        /// </summary>
        public VehicleOrderGetByDateRange VehicleOrderGetByDateRange
        {
            get { return base.GetQuery<VehicleOrderGetByDateRange>(); }
        }
    }

    #endregion // QueryAccessor

}