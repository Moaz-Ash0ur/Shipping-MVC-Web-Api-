using BL.DTOs;
using BL.Services.Shippment.ShipmentStatus;

namespace Ui.Areas.Admin.Models
{
    public class ShipmentStatusModel
    {
        public ShippmentDto shipmentDto { get; set; }
        public Guid CarrierId { get; set; }
        public DateTime DeliveryDate { get; set; } = DateTime.Today;
        public ShipmentStatusEnum NewStatus { get; set; }
    }





}

