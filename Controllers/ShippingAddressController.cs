using Ecommerce2.Data;
using Ecommerce2.Dtos;
using Ecommerce2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingAddressController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContextEf _ef;

        public ShippingAddressController(IConfiguration configuration)
        {
            _config = configuration;
            _ef = new DataContextEf(configuration);
        }

        [HttpGet("GetShippingAddress")]
        public IActionResult GetShippingAddress()
        {
            IEnumerable<ShippingAddress> shippingAddressesDb = _ef.shippingAddresses.Include(sa => sa.User).ToList();
            //Include() for specify the navigation of the relationships/foreignkeys
            return Ok(shippingAddressesDb);
        }

        [HttpPost("AddShippingAddres")]
        public IActionResult AddShippingAddress(ShippingAddressDto shippingAddressDto, int Id)
        {
            //Take all properties from that specific userId like GetSingleUser
            User? user = _ef.users.FirstOrDefault(u => u.UserId == shippingAddressDto.UserId);

            if (user == null)
            {
                return NotFound("User not found");
            }


            ShippingAddress shipping = new ShippingAddress()
            {
                AddressId = Id,
                StreetAddress = shippingAddressDto.StreetAddress,
                City = shippingAddressDto.City,
                PostalCode = shippingAddressDto.PostalCode,
                Country = shippingAddressDto.Country,
                UserId = shippingAddressDto.UserId,
                User = user,
            };
            _ef.shippingAddresses.Add(shipping);
            _ef.SaveChanges();
            return Ok(shipping);
        }
        [HttpPut("UpdateShippingAddress")]
        public IActionResult UpdateShippingAddress(int Id, ShippingAddressDto shippingAddressDto)
        {
            ShippingAddress? shipping = _ef.shippingAddresses.Where(k => k.AddressId == Id).SingleOrDefault();
            shipping.AddressId = Id;
            /*            shipping.UserId = shippingAddressDto.UserId;*/
            shipping.StreetAddress = shippingAddressDto.StreetAddress;
            shipping.City = shippingAddressDto.City;
            shipping.PostalCode = shippingAddressDto.PostalCode;
            shipping.Country = shippingAddressDto.Country;
            _ef.SaveChanges();

            return Ok();
        }

        [HttpDelete("DeleteShippingAddress")]
        public IActionResult DeleteShippingAddress(int Id)
        {
            ShippingAddress? shipping = _ef.shippingAddresses.Where(k => k.AddressId == Id).SingleOrDefault();
            _ef.shippingAddresses.Remove(shipping);
            _ef.SaveChanges();
            return Ok();
        }
    }
}
