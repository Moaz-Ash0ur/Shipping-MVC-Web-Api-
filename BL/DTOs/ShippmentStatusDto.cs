using BL.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace BL.DTOs
{
    public class ShippmentStatusDto : BaseDto
    {
        [Required(ErrorMessage = "ShippmentId is required.")]
        public Guid? ShippmentId { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }
    }





}
