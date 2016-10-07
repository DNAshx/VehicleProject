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
    /// <summary>Class for query definition and access to query called SearchPerson in evidence class Person</summary>
    /// <remarks>Query names are unique over all evidence classes.</remarks>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("QueryBuilder", "1.35.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.Diagnostics.DebuggerDisplay("ClassName={ClassName}, QueryName=SearchPerson")]
    [EvidenceQuery(UseRepoConstants = true)]
    public class SearchPerson : QueryBase
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
                return myConst.Person.CLASSNAME;
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
        internal SearchPerson(QueryFactoryContext context)
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
            base.VisitClass(myConst.Person.CLASSNAME);
            base.VisitAttribute(myConst.Person.CLASSNAME, myConst.Person.ATTR_FIRSTNAME);
            base.VisitAttribute(myConst.Person.CLASSNAME, myConst.Person.ATTR_NACHNAME);
            base.VisitAttribute(myConst.Person.CLASSNAME, myConst.Person.ATTR_ADRESSE);
            base.VisitAttribute(myConst.Person.CLASSNAME, myConst.Person.ATTR_HAUSNUMMER);
            base.VisitAttribute(myConst.Person.CLASSNAME, myConst.Person.ATTR_CITYCODE);
            base.VisitAttribute(myConst.Person.CLASSNAME, myConst.Person.ATTR_CITYNAME);

            base.CheckVisitedItemsAndThrowOnError();

            // create query tree

            MLString prmSearcnCaption = new MLString(new EvdLangId(7), "Param", new EvdLangId(7));
            EvdParameter prmSearcn = new EvdParameter("prmSearcn", ParameterType.String);
            prmSearcn.Caption = prmSearcnCaption;
            prmSearcn.RowCount = 1;

            AttributeQuery query1 = AttributeQuery.Create(Directory.get_ClassDescriptor(myConst.Person.CLASSNAME).ID);
            query1.AddRow(AttributeQueryRow.Create(Directory.get_AttrDescriptor(myConst.Person.CLASSNAME, myConst.Person.ATTR_FIRSTNAME), ComparisonOperators.LikeTo, prmSearcn, LogicalConnector.LC_OR, null, Directory.get_ClassDescriptor(myConst.Person.CLASSNAME).ID.GetValue()));
            query1.AddRow(AttributeQueryRow.Create(Directory.get_AttrDescriptor(myConst.Person.CLASSNAME, myConst.Person.ATTR_NACHNAME), ComparisonOperators.LikeTo, prmSearcn, LogicalConnector.LC_OR, null, Directory.get_ClassDescriptor(myConst.Person.CLASSNAME).ID.GetValue()));
            query1.AddRow(AttributeQueryRow.Create(Directory.get_AttrDescriptor(myConst.Person.CLASSNAME, myConst.Person.ATTR_ADRESSE), ComparisonOperators.LikeTo, prmSearcn, LogicalConnector.LC_OR, null, Directory.get_ClassDescriptor(myConst.Person.CLASSNAME).ID.GetValue()));
            query1.AddRow(AttributeQueryRow.Create(Directory.get_AttrDescriptor(myConst.Person.CLASSNAME, myConst.Person.ATTR_HAUSNUMMER), ComparisonOperators.LikeTo, prmSearcn, LogicalConnector.LC_OR, null, Directory.get_ClassDescriptor(myConst.Person.CLASSNAME).ID.GetValue()));
            query1.AddRow(AttributeQueryRow.Create(Directory.get_AttrDescriptor(myConst.Person.CLASSNAME, myConst.Person.ATTR_CITYCODE), ComparisonOperators.LikeTo, prmSearcn, LogicalConnector.LC_OR, null, Directory.get_ClassDescriptor(myConst.Person.CLASSNAME).ID.GetValue()));
            query1.AddRow(AttributeQueryRow.Create(Directory.get_AttrDescriptor(myConst.Person.CLASSNAME, myConst.Person.ATTR_CITYNAME), ComparisonOperators.LikeTo, prmSearcn, LogicalConnector.LC_OR, null, Directory.get_ClassDescriptor(myConst.Person.CLASSNAME).ID.GetValue()));

            ComplexQuery query0 = ComplexQuery.Create(query1);
            query0.Advanced = true;

            query0.Parameters.Add(prmSearcn);

            query0.GetRootNodeQuery().ParamAskingType = ParameterAskingType.AskForAll;

            return query0;

        }


        /// <summary>
        /// Returns a prepared complex query ready for the execution. Any parameters is set and checked.
        /// If a parameter is of wrong type, missing or not provided by method input it will throw an exception.
        /// This behavior is the same as already implemented in core business object.
        /// </summary>
        public ComplexQuery GetComplexQuery(string prmSearcn)
        {
            List<ParameterValue> parameters = new List<ParameterValue>();
            parameters.Add(new ParameterValue.StringParameter("prmSearcn", prmSearcn));

            //return the result of the dynamic way
            return base.GetComplexQuery(parameters.ToArray());
        }

        /// <summary>
        /// Executes query "SearchPerson" and returns result as list of business objects. 
        /// Type of business object is the type which is configured in business layer config for the 
        /// provided class id of this query.
        /// </summary>
        public List<BusinessObject> GetObjects(string prmSearcn)
        {
            List<ParameterValue> parameters = new List<ParameterValue>();
            parameters.Add(new ParameterValue.StringParameter("prmSearcn", prmSearcn));

            return base.GetObjects(parameters.ToArray());

        }

        /// <summary>
        /// Executes query "SearchPerson" and returns result as list object business objects of type T.
        /// This method can be used with LightWeigthBusinessObjects too.
        /// </summary>
        public List<T> GetObjects<T>(string prmSearcn)
            where T : CoreBusinessObject
        {
            List<ParameterValue> parameters = new List<ParameterValue>();
            parameters.Add(new ParameterValue.StringParameter("prmSearcn", prmSearcn));

            //return the result of the dynamic way
            return base.GetObjects<T>(parameters.ToArray());
        }

        /// <summary>
        /// Returns a queryable instance for query "SearchPerson". See <see cref="CoreBusinessObject.AsQueryable"/> for queryable usage.
        /// </summary>
        /// <typeparam name="BusinessObject">The business object type to get evidence class information and mapping result data to.</typeparam>
        public BusinessObjectQueryable<BusinessObject> AsQueryable<BusinessObject>(string prmSearcn)
            where BusinessObject : CoreBusinessObject
        {
            List<ParameterValue> parameters = new List<ParameterValue>();
            parameters.Add(new ParameterValue.StringParameter("prmSearcn", prmSearcn));
            return (BusinessObjectQueryable < BusinessObject>)AsQueryable<BusinessObject>(parameters.ToArray());
        }

        #endregion // Methods

    }


    #region QueryAccessor

    // static typing to support intellisense programming.
    public partial class PersonQueryAccessor : QueryAccessorBase
    {
        /// <summary>
        /// </summary>
        public SearchPerson SearchPerson
        {
            get { return base.GetQuery<SearchPerson>(); }
        }
    }

    #endregion // QueryAccessor

}