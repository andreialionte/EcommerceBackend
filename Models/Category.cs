using System.Text.Json.Serialization;

namespace Ecommerce2.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        [JsonIgnore]
        public ICollection<ProductCategory>? ProductCategory { get; set; }
    }
}
