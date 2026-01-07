using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StockMaster.API.Filters;
using StockMaster.Application.Validators;
using StockMaster.Core.Repositories;
using StockMaster.Core.Services;
using StockMaster.Core.UnitOfWorks;
using StockMaster.Infrastructure.Context;
using StockMaster.Infrastructure.Repositories;
using StockMaster.Infrastructure.UnitOfWorks;
using StockMaster.Service.Mapping;
using StockMaster.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // Filter'ý tüm controllerlara uyguluyoruz
    options.Filters.Add<ValidateFilterAttribute>();
});

// .NET'in kendi varsayýlan validasyon cevabýný kapatýyoruz (Çünkü biz Filter ile yöneteceðiz)
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddFluentValidationAutoValidation(); // Otomatik validasyonu aç
builder.Services.AddValidatorsFromAssemblyContaining<ProductDtoValidator>(); // Validator'larýn olduðu yeri göster

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL Veritabaný Baðlantýsý
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"), npgsqlOptions =>
    {
        // Migration dosyalarýnýn "StockMaster.Infrastructure" projesinde oluþacaðýný belirtiyoruz.
        npgsqlOptions.MigrationsAssembly("StockMaster.Infrastructure");
    });
});

// Generic Repository ve UnitOfWork Kayýtlarý
// Scoped: Her HTTP isteði için bir tane nesne üretir.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// AutoMapper'ý kaydet (MapProfile'ýn olduðu assembly'i veriyoruz)
builder.Services.AddAutoMapper(typeof(MapProfile));

// Generic Service Kaydý
builder.Services.AddScoped(typeof(IService<,>), typeof(Service<,>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();