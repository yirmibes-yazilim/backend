using AutoMapper;
using backend.Application.DTOs.Auth;
using backend.Application.DTOs.Category;
using backend.Application.DTOs.Product;
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
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Product, GetProductResponseDto>();

            CreateMap<CreateCategoryRequestDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Category, GetCategoryResponseDto>();

            CreateMap<UpdateProductRequestDto, Product>();
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
        }
    }
}
