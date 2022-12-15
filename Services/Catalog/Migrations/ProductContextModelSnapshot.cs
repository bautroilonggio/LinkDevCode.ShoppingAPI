﻿// <auto-generated />
using System;
using Catalog.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Catalog.Migrations
{
    [DbContext(typeof(ProductContext))]
    partial class ProductContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Catalog.DataAccess.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double?>("SellingPrice")
                        .HasColumnType("float");

                    b.Property<int>("TotalQuantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Brand = "Samsung",
                            Category = "Smartphone",
                            CreateAt = new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "8GB/128GB",
                            Name = "Galaxy S21 FE (5G)",
                            SellingPrice = 11850000.0,
                            TotalQuantity = 20
                        },
                        new
                        {
                            Id = 2,
                            Brand = "Samsung",
                            Category = "Smartphone",
                            CreateAt = new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "12GB/512GB",
                            Name = "Galaxy S22 Ultra",
                            SellingPrice = 28990000.0,
                            TotalQuantity = 25
                        },
                        new
                        {
                            Id = 3,
                            Brand = "Samsung",
                            Category = "Smartwatch",
                            CreateAt = new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "45mm",
                            Name = "Galaxy Watch 5 Pro",
                            SellingPrice = 10990000.0,
                            TotalQuantity = 21
                        },
                        new
                        {
                            Id = 4,
                            Brand = "Samsung",
                            Category = "Bluetooth Headphone",
                            CreateAt = new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "In-Ear",
                            Name = "Galaxy Buds 2 Pro",
                            SellingPrice = 4590000.0,
                            TotalQuantity = 14
                        },
                        new
                        {
                            Id = 5,
                            Brand = "Samsung",
                            Category = "Bluetooth Speaker",
                            CreateAt = new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Sound Tower",
                            Name = "Samsung Sound Tower ST50B/XV",
                            SellingPrice = 8490000.0,
                            TotalQuantity = 18
                        },
                        new
                        {
                            Id = 6,
                            Brand = "Samsung",
                            Category = "Tablet",
                            CreateAt = new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "8B/128GB",
                            Name = "Galaxy Tab S8 Ultra",
                            SellingPrice = 25390000.0,
                            TotalQuantity = 12
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
