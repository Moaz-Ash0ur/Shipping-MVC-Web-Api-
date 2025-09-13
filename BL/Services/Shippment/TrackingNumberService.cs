using BL.Contracts.Shippment;
using BL.DTOs;
using System.Transactions;

namespace BL.Services.Shippment
{
    public class TrackingNumberService : ITrackingNumberCreater
    {
        public string Create()
        {
            string prefix = "TRK";
            string datePart = DateTime.Now.ToString("yyyyMMdd");
            int randomPart = new Random().Next(1000, 9999);

            return $"{prefix}{datePart}{randomPart}";
        }
    } 


}
