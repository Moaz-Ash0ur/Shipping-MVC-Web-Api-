using BL.Contracts.Shippment;
using BL.DTOs;
using System.Transactions;

namespace BL.Services.Shippment
{
    public class RateCalculationService : IRateCalculater
    {
        public decimal Calculate(ShippmentDto shippmentDto)
        {
            double basePrice = 50;
            if (shippmentDto.Weight > 5)
            {
                basePrice += (shippmentDto.Weight - 5) * 10;
            }
            return Convert.ToDecimal(basePrice);
        }

    }

    public class WeightBasedRateStrategy 
    {
        public double Calculate(ShippmentDto shippmentDto)
        {
            double basePrice = 50; 
            if (shippmentDto.Weight > 5)
            {
                basePrice += (shippmentDto.Weight - 5) * 10;
            }
            return basePrice;
        }
    }



}
