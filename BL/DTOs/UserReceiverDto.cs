using BL.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace BL.DTOs
{
    public class UserReceiverDto : BaseDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Receiver name is required.")]
        [StringLength(100, ErrorMessage = "Receiver name cannot exceed 100 characters.")]
        public string ReceiverName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; } = null!;

        [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters.")]
        public string PostalCode { get; set; }

        [StringLength(100, ErrorMessage = "Contact cannot exceed 100 characters.")]
        public string Contact { get; set; } = null!;

        [StringLength(250, ErrorMessage = "Other address cannot exceed 250 characters.")]
        public string OtherAddress { get; set; } = null!;

        public bool IsDefault { get; set; }

        [Required(ErrorMessage = "CityId is required.")]
        public Guid CityId { get; set; }

        [Required(ErrorMessage = "CountryId is required.")]
        public Guid CountryId { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; } = null!;
    }




}
