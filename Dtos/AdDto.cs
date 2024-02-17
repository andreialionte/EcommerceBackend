namespace Ecommerce2.Dtos
{
    public class AdDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        public IFormFile? PhotoUrl { get; set; }
        /*        public int ProductId { get; set; }*/
    }
}
