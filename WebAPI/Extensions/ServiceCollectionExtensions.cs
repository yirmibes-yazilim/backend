using API.Infrastructure.Profiles;
using Azure.Storage.Blobs;
using backend.Application.Services;
using backend.Application.Validator;
using backend.Infrastructure.Data;
using backend.Infrastructure.Repositories;
using backend.Loger;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

namespace backend.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            // Register application services  
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<ProductValidator>();
            services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
            services.AddValidatorsFromAssemblyContaining<RatingProductValidator>();

            services.AddScoped<DbContext, AppDbContext>();
            services.AddScoped<IUnitofWork, UnitofWork>();
            services.AddScoped(typeof(IService<>), typeof(Service<>));

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICardItemService, CardItemService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddSingleton<IMyLogger, MyLogger>();
            services.AddScoped<IFavoriteProductService, FavoriteProductService>();
            services.AddScoped<IRatingProductService, RatingProductService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton(sp =>
            {
                var cfg = sp.GetRequiredService<IConfiguration>();
                var conn = cfg["AzureBlobStorage:ConnectionString"];
                var container = cfg["AzureBlobStorage:ContainerName"];
                return new BlobContainerClient(conn, container);
            });

            //Azure Blob Storage Service
            services.AddScoped<IBlobService, BlobService>();

            // Configure rate limiting
            services.AddRateLimiter(options =>
            {
                options.AddPolicy("IpBasedPolicy", context =>
                {
                    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown-ip";

                    return RateLimitPartition.GetFixedWindowLimiter(ip, key => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 1,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });
                });
            });
            return services;
        }
    }
}
