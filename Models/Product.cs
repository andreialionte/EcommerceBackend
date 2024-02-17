using Ecommerce2.Models;
using System.Text.Json.Serialization;

namespace Ecommerce.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        /*        public string Description { get; set; }*/
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        [JsonIgnore]
        public ICollection<ProductCategory>? ProductCategory { get; set; }
        public Ad? Ads { get; set; }
    }
}
