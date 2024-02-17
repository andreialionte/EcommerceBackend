using Ecommerce.Models;
using Ecommerce2.Data;
using Ecommerce2.Dtos;
using Ecommerce2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce2.Controllers
{
    public class OrderItemController : ControllerBase
    {
        private readonly DataContextEf _ef;
        private readonly IConfiguration _config;
        private readonly ILogger<OrderItem> _logger;
        public OrderItemController(IConfiguration configuration, ILogger<OrderItem> logger)
        {
            _config = configuration;
            _ef = new DataContextEf(configuration);
            _logger = logger;
        }

        [HttpGet("GetOrderItem")]
        public IActionResult GetOrderItem()
        {
            IEnumerable<OrderItem> orderItemsDb = _ef.orderItems.Include(u => u.Order).ThenInclude(order => order.User).Include(u => u.Product).ToList();
            return Ok(orderItemsDb);
        }
        [HttpPost("PostOrderItem")]
        public IActionResult PostOrderItem(OrderItemDto orderItemDto)
        {
            //here we need to handle from 0 the OrderController to get and make a new Order becasue
            //we cant handle the TotalAmount in OrderController so we will need to copy paste the code from
            //from http post OrderController here and make the calculate the total price
            //like for just updating the total amount instantly
            Order? orderDb = _ef.orders.Where(k => k.OrderId == orderItemDto.OrderId).FirstOrDefault();
            Product? productDb = _ef.products.Where(k => k.ProductId == orderItemDto.ProductId).FirstOrDefault();


            OrderItem? orderItemsDb = new OrderItem
            {
                OrderId = orderDb.OrderId,
                ProductId = productDb.ProductId,
                Quantity = orderItemDto.Quantity,
            };

            decimal subtotal = productDb.Price * orderItemDto.Quantity;
            orderDb.TotalAmount = orderDb.TotalAmount + subtotal; //ofc or orderDb.TotalAmount += subtotal (to be equal)

            _ef.orderItems.Add(orderItemsDb);
            productDb.StockQuantity = productDb.StockQuantity - orderItemDto.Quantity;

            if (productDb.StockQuantity < orderItemDto.Quantity)
            {
                _logger.LogError($"Insufficient stock for the products: {productDb.Name}");
                return NotFound($"Insufficient stock for the products: {productDb.Name}");
            }
            _ef.SaveChanges();

            if (orderItemsDb.OrderId != orderDb.OrderId || orderItemsDb.ProductId != productDb.ProductId)
            {
                return NotFound("OrderID or ProductId dosent exist");
            }
            return Ok(orderItemsDb);
        }
        [HttpPut("UpdateOrderItem")]
        public IActionResult UpdateOrderItem()
        {
            return Ok();
        }
        [HttpDelete("DeleteOrderItem")]
        public IActionResult DeleteOrderItem()
        {
            return Ok();
        }
    }
}
