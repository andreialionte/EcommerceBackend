﻿namespace Ecommerce2.Dtos
{
    public class OrderDto
    {
        /*        public int OrderId { get; set; }*/
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
