using Microsoft.EntityFrameworkCore;
using StockMaster.Infrastructure.Context;

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