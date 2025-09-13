namespace Domains;


    public partial class TbShippingPackage : BaseTable
    {
        public string? ShipingPackgingAname { get; set; }

        public string? ShipingPackgingEname { get; set; }

        public virtual ICollection<TbShippment> TbShippments { get; set; } = new List<TbShippment>();
    }
