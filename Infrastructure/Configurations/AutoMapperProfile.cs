﻿using AutoMapper;
using backend.Application.DTOs.Addresses;
using backend.Application.DTOs.Auth;
using backend.Application.DTOs.CardItem;
using backend.Application.DTOs.Category;
using backend.Application.DTOs.FavoriteProduct;
using backend.Application.DTOs.Order;
using backend.Application.DTOs.OrderItem;
using backend.Application.DTOs.Product;
using backend.Application.DTOs.RatingProduct;
using backend.Application.DTOs.UserRole;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;


namespace API.Infrastructure.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateProductRequestDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<Product, GetProductResponseDto>();

            CreateMap<CreateCategoryRequestDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Category, GetCategoryResponseDto>();

            CreateMap<UpdateProductRequestDto, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<UpdateCategoryRequestDto, Category>();

            CreateMap<User, UserResponseDto>();
            CreateMap<RegisterRequestDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    dest.PasswordHash = context.Items["PasswordHash"] as string ?? "";
                });

            CreateMap<CreateUserRoleRequest, UserRole>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UpdateUserRoleRequest, UserRole>();
            CreateMap<UserRole, GetUserRoleResponse>();

            CreateMap<CreateAddressesRequestDto, Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDefault, opt => opt.Ignore());

            CreateMap<Address, GetAddressesResponseDto>();
            CreateMap<UpdateAddressesRequestDto, Address>()
                .ForMember(dest => dest.IsDefault, opt => opt.Ignore());

            CreateMap<CreateCardItemRequestDto, CardItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<CardItem, GetCardItemResponseDto>();

            CreateMap<Order, GetOrderResponseDto>();
            CreateMap<OrderItem, GetOrderItemResponseDto>();    

            CreateMap<CreateFavoriteProductRequestDto, FavoriteProduct>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<RatingProduct, GetRatingProductResponseDto> ()
                .ReverseMap();
            CreateMap<UpdateRatingProductRequest, RatingProduct>();
            CreateMap<CreateRatingProductRequestDto, RatingProduct>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
