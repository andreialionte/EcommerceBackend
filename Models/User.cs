using System.Text.Json.Serialization;

namespace Ecommerce2.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public DateTime RegistrationDate { get; set; }

        // Add a collection navigation property for Orders
        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; } //manipulate, for foreignkeys from db
        //represents a navigation property that allows you to navigate
        //from a User object to a collection(Order) of associated Order objects.
        //This property is typically used to establish relationships
        //between entities in an Entity Framework Core data model.
        //represents a one-to-many relationship between a User and multiple Order entities
        [JsonIgnore]
        public ICollection<ShippingAddress>? ShippingAddresses { get; set; }
    }
}
