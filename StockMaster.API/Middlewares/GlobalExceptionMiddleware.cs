using Microsoft.AspNetCore.Diagnostics;
using StockMaster.Core.DTOs;
using System.Net;
using System.Text.Json;

namespace StockMaster.API.Middlewares
{
    // Bu sınıfı doğrudan Middleware olarak değil, Extension method içinde kullanacağız.
    // .NET Core'un kendi "UseExceptionHandler" mekanizmasını modifiye ediyoruz.
    public static class CustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                // Hata olduğunda çalışacak kod bloğu
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    // Hata özelliğini yakala
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    // Varsayılan durum kodu: 500
                    var statusCode = 500;

                    if (exceptionFeature != null)
                    {
                        var ex = exceptionFeature.Error;

                        // İleride buraya "if (ex is ClientSideException)" gibi kontroller ekleyerek
                        // 400, 404 gibi kodları ayrıştırabiliriz.

                        // Cevabı hazırla
                        context.Response.StatusCode = statusCode;

                        var response = CustomResponseDto<NoContentDto>.Fail(statusCode, ex.Message);

                        // JSON'a çevirip gönder
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });
            });
        }
    }
}