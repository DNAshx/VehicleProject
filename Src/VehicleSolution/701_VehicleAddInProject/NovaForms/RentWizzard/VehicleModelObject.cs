using Evidence.Business;
using Evidence.Nova.Common;
using GlauxSoft.GreenTransport.Repository;

namespace GreenTransport.NovaForms.RentWizzard
{
    public sealed class VehicleModelObject : Evidence.Nova.Common.ViewModelObject
    {
        public EvdObjectId VehicleId
        {
            get { return (EvdObjectId)GetValue(); }
            set { SetValue(value); }
        }
        public bool Selected
        {
            get { return (bool)GetValue(); }
            set { SetValue(value); }
        }
        public string Brand
        {
            get { return (string)GetValue(); }
            set { SetValue(value); }
        }
        public int QtyPassengers
        {
            get { return (int)GetValue(); }
            set { SetValue(value); }
        }
        public double PriceDay
        {
            get { return (double)GetValue(); }
            set { SetValue(value); }
        }
        public VehicleModelObject(Vehicle vehicle)
        {
            this.VehicleId = vehicle.ObjectID;
            this.Brand = vehicle.Brand;
            this.QtyPassengers = vehicle.QtPassengers.GetValueOrDefault(1);
            this.PriceDay = vehicle.PriceDay.GetValueOrDefault(0);
            this.Selected = false;
        }
    }
}