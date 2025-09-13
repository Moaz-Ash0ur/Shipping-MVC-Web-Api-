using BL.DTOs;

namespace BL.Contracts.Shippment
{
    public interface IRateCalculater
    {
        //sont need to send dto all | i send beacuse i deont track number depand for it
        //shoud send data this depand to create
        public decimal Calculate(ShippmentDto shippmentDto);

    }

}
