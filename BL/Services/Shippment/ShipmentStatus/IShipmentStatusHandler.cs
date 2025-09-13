using BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Shippment.ShipmentStatus
{
    public interface IShipmentStatusHandler
    {
        public ShipmentStatusEnum TargetState { get; }
        public Task HandelState(ShippmentDto shippmentDto);
    }





}
