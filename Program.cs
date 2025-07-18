using API.Infrastructure.Profiles;
using Azure.Storage.Blobs;
using backend.Application.DTOs.Auth;
using backend.Application.DTOs.UserRole;
using backend.Application.Services;
using backend.Application.Validator;
using backend.Domain.Entities;
using backend.Infrastructure.Data;
using backend.Infrastructure.Repositories;
using backend.Loger;
using backend.WebAPI.Extensions;
using backend.WebAPI.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Parse("192.168.25.122"), 5102);
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce API", Version = "v1" });

    // JWT Authentication için taným
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddCustomServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<IService<User>>();
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
    var roleService = scope.ServiceProvider.GetRequiredService<IUserRoleService>();

    var adminEmail = "admin@admin.com";
    var adminUser = await service.GetFirstOrDefaultAsync(u => u.Email == adminEmail);

    if (adminUser == null)
    {
        await authService.RegisterAsync(new RegisterRequestDto
        {
            Username = "admin",
            FirstName = "admin",
            LastName = "admin",
            Email = adminEmail,
            Password = "Admin1234!",
        });
        var admin = await service.GetFirstOrDefaultAsync(u => u.Email == adminEmail);
        await roleService.AddUserRoleAsync(new CreateUserRoleRequest
        {
            UserId = admin.Id,
            Role = "Admin"
        });
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseMiddleware<LogMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers().RequireRateLimiting("IpBasedPolicy");

app.Run();
