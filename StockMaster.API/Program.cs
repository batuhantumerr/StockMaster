using Microsoft.EntityFrameworkCore;
using StockMaster.Core.Repositories;
using StockMaster.Service.Mapping;
using StockMaster.Core.Services;
using StockMaster.Core.UnitOfWorks;
using StockMaster.Infrastructure.Context;
using StockMaster.Infrastructure.Repositories;
using StockMaster.Infrastructure.UnitOfWorks;
using StockMaster.Service.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

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
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();