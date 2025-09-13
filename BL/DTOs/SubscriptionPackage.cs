using BL.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace BL.DTOs
{
    public class SubscriptionPackageDto : BaseDto
    {
        [Required(ErrorMessage = "Package Name is required.")]
        [StringLength(100, ErrorMessage = "Package Name cannot exceed 100 characters.")]
        public string PackageName { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Shipment count must be at least 1.")]
        public int ShippimentCount { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Number of kilometers must be greater than 0.")]
        public double NumberOfKiloMeters { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Total weight must be greater than 0.")]
        public double TotalWeight { get; set; }
    }




}
