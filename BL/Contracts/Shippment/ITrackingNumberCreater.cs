using BL.DTOs;

namespace BL.Contracts.Shippment
{
    public interface ITrackingNumberCreater
    {
        //sont need to send dto all | i send beacuse i deont track number depand for it
        //shoud send data this depand to create
        public string Create();

    }

}
