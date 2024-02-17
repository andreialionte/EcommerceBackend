using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Ecommerce2.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; } // Navigation property for the User //and it shows all the data from Model.User
        //represents the relationship between an Order and a User
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        [JsonIgnore]
        public ICollection<OrderItem>? Items { get; set; }
    }
}

