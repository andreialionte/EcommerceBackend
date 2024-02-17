using Ecommerce2.Data;
using Ecommerce2.Dtos;
using Ecommerce2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly DataContextEf _ef;
        private readonly IConfiguration _config;
        /*        private readonly OrderService _order;*/

        public OrderController(IConfiguration configuration/* OrderService order*/)
        {
            _config = configuration;
            _ef = new DataContextEf(configuration);
            /*            _order = order;*/
        }

        [HttpGet("GetOrders")]
        public IActionResult GetOrders()
        {
            IEnumerable<Order> orders = _ef.orders.Include(u => u.User).ToList();
            return Ok(orders);
        }

        [HttpPost("AddOrders")]
        public IActionResult AddOrders(OrderDto orderDto, int Id)
        {
            User? userDb = _ef.users.FirstOrDefault(u => u.UserId == orderDto.UserId);

            if (userDb == null)
            {
                return NotFound("User Doesn't Exist!");
            }

            decimal totalAmount = 0.0m;

            Order? orderDb = new Order()
            {
                OrderId = Id,
                UserId = orderDto.UserId,
                User = userDb,
                OrderDate = orderDto.OrderDate,
                TotalAmount = totalAmount,
            };
            var orderItems = _ef.orderItems.Where(oi => oi.OrderId == Id).ToList();

            foreach (var orderItem in orderItems)
            {
                var product = _ef.products.FirstOrDefault(p => p.ProductId == orderItem.ProductId);

                if (product != null)
                {
                    decimal subtotal = product.Price * orderItem.Quantity;

                    totalAmount = totalAmount + subtotal;
                }
            }

            orderDb.TotalAmount = totalAmount;

            _ef.orders.Add(orderDb);
            _ef.SaveChanges();
            return Ok(orderDb);
        }


        [HttpPut("UpdateOrders")]
        public IActionResult UpdateOrders(OrderDto orderDto, int Id)
        {
            Order? orderDb = _ef.orders.FirstOrDefault(k => k.OrderId == Id);
            if (orderDb != null)
            {
                orderDb.OrderDate = orderDto.OrderDate;
                orderDb.OrderId = Id;
                orderDb.UserId = orderDto.UserId;
                orderDb.TotalAmount = orderDto.TotalAmount;
                _ef.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound($"{orderDb.OrderId} not found");
            }
        }

        [HttpDelete("DeleteOrders")]
        public IActionResult RemoveOrders(int Id)
        {
            Order? orderDb = _ef.orders.FirstOrDefault(k => k.OrderId == Id);
            if (orderDb != null)
            {
                _ef.orders.Remove(orderDb);
                _ef.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
