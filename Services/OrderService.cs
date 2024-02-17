/*using Ecommerce2.Data;

namespace Ecommerce2.Services
{
    public class OrderService
    {
        private readonly IConfiguration _config;
        private readonly DataContextEf _ef;
        public OrderService(IConfiguration configuration)
        {
            _config = configuration;
            _ef = new DataContextEf(configuration);
        }

        public decimal CalculateTotalAmount(int orderId)
        {
            var orderItemDb = _ef.orderItems.Where(o => o.OrderId == orderId).ToList();
            decimal totalAmount = 0;

            foreach (var item in orderItemDb)
            {
                var product = _ef.products.Where(o => o.ProductId == item.ProductId).FirstOrDefault();
                if (product != null)
                {
                    totalAmount = totalAmount + product.Price * item.Quantity;
                }
            }
            return totalAmount;
        }
    }
}
*/