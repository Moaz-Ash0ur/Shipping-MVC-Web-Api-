namespace Domains;

public class ShipmentFlat
{
    public Guid Id { get; set; }
    public DateTime ShippingDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public Guid ShippingTypeId { get; set; }
    public Guid? ShippingPackgingId { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public double Length { get; set; }
    public decimal PackageValue { get; set; }
    public decimal ShippingRate { get; set; }
    public Guid? PaymentMethodId { get; set; }
    public Guid? UserSubscriptionId { get; set; }
    public string? TrackingNumber { get; set; }
    public Guid? ReferenceId { get; set; }
    public int? CurrentState { get; set; }

    // Sender Info
    public Guid SenderUserId { get; set; }
    public string SenderName { get; set; }
    public string SenderEmail { get; set; }
    public string SenderPhone { get; set; }
    public string SenderPostalCode { get; set; }
    public string SenderContact { get; set; }
    public string SenderOtherAddress { get; set; }
    public bool SenderIsDefault { get; set; }
    public Guid SenderCityId { get; set; }
    public Guid SenderCountryId { get; set; }
    public string SenderAddress { get; set; }

    // Receiver Info
    public Guid ReceiverUserId { get; set; }
    public string ReceiverName { get; set; }
    public string ReceiverEmail { get; set; }
    public string ReceiverPhone { get; set; }
    public string ReceiverPostalCode { get; set; }
    public string ReceiverContact { get; set; }
    public string ReceiverOtherAddress { get; set; }
    public bool ReceiverIsDefault { get; set; }
    public Guid ReceiverCityId { get; set; }
    public Guid ReceiverCountryId { get; set; }
    public string ReceiverAddress { get; set; }
}

