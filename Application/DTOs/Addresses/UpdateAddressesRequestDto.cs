﻿namespace backend.Application.DTOs.Addresses
{
    public class UpdateAddressesRequestDto
    {
        public int Id { get; set; }
        //public int UserId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

    }
}
