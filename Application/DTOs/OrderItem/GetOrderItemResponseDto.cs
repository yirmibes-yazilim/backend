﻿namespace backend.Application.DTOs.OrderItem
{
    public class GetOrderItemResponseDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}