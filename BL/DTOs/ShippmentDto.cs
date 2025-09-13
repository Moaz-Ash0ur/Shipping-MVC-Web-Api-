using BL.DTOs.Base;
using Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs
{
    public class ShippmentDto : BaseDto
    {
        [Required(ErrorMessage = "Shipping Date is required.")]
        public DateTime ShippingDate { get; set; }

        [Required(ErrorMessage = "Delivery Date is required.")]
        public DateTime DeliveryDate { get; set; }

        [Required(ErrorMessage = "SenderId is required.")]
        public Guid SenderId { get; set; }

        [Required(ErrorMessage = "Sender info is required.")]
        public UserSenderDto UserSender { get; set; }

        [Required(ErrorMessage = "ReceiverId is required.")]
        public Guid ReceiverId { get; set; }

        [Required(ErrorMessage = "Receiver info is required.")]
        public UserReceiverDto UserReceiver { get; set; }

        [Required(ErrorMessage = "Shipping Type is required.")]
        public Guid ShippingTypeId { get; set; }

        public Guid? ShippingPackgingId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Width must be greater than 0.")]
        public double Width { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Height must be greater than 0.")]
        public double Height { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Weight must be greater than 0.")]
        public double Weight { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Length must be greater than 0.")]
        public double Length { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Package Value must be greater than 0.")]
        public decimal PackageValue { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Shipping Rate must be greater than 0.")]
        public decimal ShippingRate { get; set; }

        public Guid? PaymentMethodId { get; set; }

        public Guid? UserSubscriptionId { get; set; }

        [StringLength(50, ErrorMessage = "Tracking number cannot exceed 50 characters.")]
        public string? TrackingNumber { get; set; }

        public Guid? ReferenceId { get; set; }

        public Guid? CarrierId { get; set; }

        [Range(0, 9, ErrorMessage = "CurrentState must be a valid state.")]
        public int CurrentState { get; set; }
    }



}
