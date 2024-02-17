using Ecommerce.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Ecommerce2.Models
{
    public class Ad
    {
        /*        [Key]
                [DatabaseGenerated(DatabaseGeneratedOption.Identity)]*/
        public int AnuntId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }

        [ForeignKey("ProductId")]
        [JsonIgnore]
        public Product? Product { get; set; }
        public int ProductId { get; set; }

    }
}
