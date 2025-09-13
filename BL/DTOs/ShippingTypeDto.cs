using BL.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace BL.DTOs
{
    public class ShippingTypeDto : BaseDto
    {
        [Required(ErrorMessage = "Arabic Name is required.")]
        [StringLength(100, ErrorMessage = "Arabic Name cannot exceed 100 characters.")]
        public string? ShippingTypeAname { get; set; }

        [Required(ErrorMessage = "English Name is required.")]
        [StringLength(100, ErrorMessage = "English Name cannot exceed 100 characters.")]
        public string? ShippingTypeEname { get; set; }

        [Required(ErrorMessage = "Shipping Factor is required.")]
        [Range(0.25, 10, ErrorMessage = "Shipping Factor must be between 0.01 and 10.")]
        public double ShippingFactor { get; set; }

    }





}
