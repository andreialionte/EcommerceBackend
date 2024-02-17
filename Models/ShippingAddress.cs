using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce2.Models
{
    public class ShippingAddress
    {
        public int AddressId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public int UserId { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

    }
}
