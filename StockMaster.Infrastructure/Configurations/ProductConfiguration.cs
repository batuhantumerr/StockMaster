using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockMaster.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockMaster.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Name: Zorunlu, Max 200 karakter
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

            // Stock: Zorunlu
            builder.Property(x => x.Stock).IsRequired();

            // Price: Zorunlu
            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");

            // İlişki: Bir ürünün bir kategorisi olur
            builder.HasOne(x => x.Category)
                   .WithMany(x => x.Products)
                   .HasForeignKey(x => x.CategoryId);
        }
    }
}
