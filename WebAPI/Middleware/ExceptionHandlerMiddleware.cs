using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.WebAPI.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    ValidationException => 400,
                    ArgumentNullException => 400,
                    UnauthorizedAccessException => 401,
                    SecurityTokenExpiredException => 401,
                    KeyNotFoundException => 404,
                    _ => 500
                };
                var response = Response<NoContent>.Fail("Something went wrong! " + ex.Message, (HttpStatusCode)context.Response.StatusCode);
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }

        }
    }
}