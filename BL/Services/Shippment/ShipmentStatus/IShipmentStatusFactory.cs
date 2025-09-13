namespace BL.Services.Shippment.ShipmentStatus
{
    public interface IShipmentStatusFactory
    {
     public IShipmentStatusHandler GetHandler(ShipmentStatusEnum shipmentStatusEnum);
    }




}
