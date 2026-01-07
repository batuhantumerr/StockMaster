using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using StockMaster.API.Filters;
using StockMaster.API.Middlewares;
using StockMaster.Application.Validators;
using StockMaster.Core.Repositories;
using StockMaster.Core.Services;
using StockMaster.Core.UnitOfWorks;
using StockMaster.Infrastructure.Context;
using StockMaster.Infrastructure.Repositories;
using StockMaster.Infrastructure.UnitOfWorks;
using StockMaster.Service.Mapping;
using StockMaster.Service.Services;
using System.Text;
using Microsoft.OpenApi.Models;

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockMaster API", Version = "v1" });

    // 1. "Authorize" kutucuðunu tanýmlýyoruz
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
    });

    // 2. Bu güvenliði tüm endpointlere zorunlu kýlýyoruz (Kilit simgesi çýksýn)
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

// PostgreSQL Veritabaný Baðlantýsý
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"), npgsqlOptions =>
    {
        // Migration dosyalarýnýn "StockMaster.Infrastructure" projesinde oluþacaðýný belirtiyoruz.
        npgsqlOptions.MigrationsAssembly("StockMaster.Infrastructure");
    });
});

// AuthService'i Kaydet
builder.Services.AddScoped<AuthService>();

// Authentication (Kimlik Doðrulama) Ayarlarý
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["TokenOptions:Issuer"],
            ValidAudience = builder.Configuration["TokenOptions:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenOptions:SecurityKey"]))
        };
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

app.UseCustomException();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();