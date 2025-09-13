using BL.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace BL.DTOs
{
    public class CarrierDto : BaseDto
    {
        [Required(ErrorMessage = "Carrier Name is required.")]
        [StringLength(100, ErrorMessage = "Carrier Name cannot exceed 100 characters.")]
        public string CarrierName { get; set; } = null!;
    }


}
