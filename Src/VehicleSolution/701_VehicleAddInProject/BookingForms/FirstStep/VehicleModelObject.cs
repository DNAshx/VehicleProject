using Evidence.Business;
using Evidence.Nova.Common;
using GlauxSoft.GreenTransport.Repository;

namespace GreenTransport.BookingForms.FirstStep
{
    public sealed class VehicleModelObject : Evidence.Nova.Common.ViewModelObject
    {
        public string Brand { get; set; }
        public int QtyPassengers { get; set; }
        public double PriceDay { get; set; }
        public VehicleModelObject(Vehicle vehicle)
        {
            this.Brand = vehicle.Brand;
            this.QtyPassengers = vehicle.QtPassengers.GetValueOrDefault(1);
            this.PriceDay = vehicle.PriceDay.GetValueOrDefault(0);
        }
    }
}