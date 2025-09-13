using BL.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace BL.DTOs
{
    public class ShippingPackageDto : BaseDto
    {
        [Required(ErrorMessage = "Arabic name is required.")]
        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters.")]
        public string? ShipingPackgingAname { get; set; }

        [Required(ErrorMessage = "English name is required.")]
        [StringLength(100, ErrorMessage = "English name cannot exceed 100 characters.")]
        public string? ShipingPackgingEname { get; set; }
    }


}
