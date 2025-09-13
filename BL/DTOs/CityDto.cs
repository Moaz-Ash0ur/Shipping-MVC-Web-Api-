using BL.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace BL.DTOs
{
    public class CityDto : BaseDto
    {
        [Required(ErrorMessage = "Arabic Name is required.")]
        [StringLength(100, ErrorMessage = "Arabic Name cannot exceed 100 characters.")]
        public string? CityAname { get; set; }
        
        [Required(ErrorMessage = "English Name is required.")]
        [StringLength(100, ErrorMessage = "English Name cannot exceed 100 characters.")]      
        public string? CityEname { get; set; }
        
        [Display(Name = "Country Name")]
        [Required(ErrorMessage = "{0} is required")]
        public Guid CountryId { get; set; }
               

        public string? CountryAname { get; set; }           
        public string? CountryEname { get; set; }
    }



}
