namespace Ecommerce2.Dtos
{
    public class UserDto
    {
        /*        public int UserId { get; set; }*/
        public string? Username { get; set; }
        public string? Email { get; set; }
        /*        public DateTime RegistrationDate { get; set; }*/

        // Add a collection navigation property for Orders
        /*        public ICollection<Order> Orders { get; set; }*/
    }
}
