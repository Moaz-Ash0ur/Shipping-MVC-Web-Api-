using BL.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace BL.DTOs
{
    public class UserSubscriptionDto : BaseDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "PackageId is required.")]
        public Guid PackageId { get; set; }

        [Required(ErrorMessage = "Subscription date is required.")]
        public DateTime SubscriptionDate { get; set; }

        public SubscriptionPackageDto? Package { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Remaining shipments must be 0 or more.")]
        public int? RemainingShipments { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Remaining kilometers must be 0 or more.")]
        public double? RemainingKm { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Remaining weight must be 0 or more.")]
        public double? RemainingWeight { get; set; }
    }




}
