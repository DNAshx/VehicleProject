using GlauxSoft.Business;

namespace GlauxSoft.GreenTransport.Queries
{
    public partial class QueryFactory : QueryFactoryBase
    {
        //################################################################
        //# THIS CODE WAS AUTOGENERATED CODE BY THE QUERYBUILDER TOOL.   # 
        //# MAKE SURE YOU KNOW WHAT YOU'RE DOING WHEN EDITING MANUALLY!  #
        //################################################################
        
        public static LocationQueryAccessor Location
        {
            get
            {
                return QueryFactoryBase.GetAccessor<LocationQueryAccessor>();
            }
        }
    }
}
