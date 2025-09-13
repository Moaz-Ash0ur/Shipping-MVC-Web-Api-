using Microsoft.Extensions.DependencyInjection;

namespace BL.Services.Shippment.ShipmentStatus
{
    public class ShipmentStatusFactory : IShipmentStatusFactory
    {
        private IServiceProvider _serviceProvider;

        public ShipmentStatusFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IShipmentStatusHandler GetHandler(ShipmentStatusEnum status)
        {
            return status switch
            {
                ShipmentStatusEnum.Approved => _serviceProvider.GetRequiredService<ApprovedShipment>(),
                ShipmentStatusEnum.ReadyForShip => _serviceProvider.GetRequiredService<ReadyForShip>(),
                ShipmentStatusEnum.Shipped => _serviceProvider.GetRequiredService<Shipped>(),
                ShipmentStatusEnum.Delivered => _serviceProvider.GetRequiredService<Delivered>(),
                ShipmentStatusEnum.Returned => _serviceProvider.GetRequiredService<Returned>(),
                _ => throw new ArgumentException("Invalid Shipment Status")
            };
        }
    }




}
