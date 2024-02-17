namespace Ecommerce2.Dtos
{
    public class ShippingAddressDto
    {
        /*        public int AddressId { get; set; }*/
        /*        [ForeignKey("UserId")]
                public User? User { get; set; }*/
        public int UserId { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
}
