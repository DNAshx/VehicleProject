using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlauxSoft.Vehicle.Common
{
    public class Vehicle : GlauxSoft.GreenTransport.Repository.Vehicle
    { 
        public List<string> VehicleType
        {
            get;
            set;
        }
    }
}
