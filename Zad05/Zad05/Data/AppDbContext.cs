using Microsoft.EntityFrameworkCore;
using Zad05.Models;

namespace Zad05.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Pc> Pcs { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<PcComponent> PcComponents { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<ComponentManufacturer> ComponentManufacturers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pc>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(50);
            e.Property(x => x.Weight).IsRequired();
            e.Property(x => x.Warranty).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired().HasColumnType("datetime");
            e.Property(x => x.Stock).IsRequired();
        });

        modelBuilder.Entity<Component>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.Property(x => x.Description).IsRequired();

            e.HasOne(x => x.ComponentManufacturer)
                .WithMany(x => x.Components)
                .HasForeignKey(x => x.ComponentManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.ComponentType)
                .WithMany(x => x.Components)
                .HasForeignKey(x => x.ComponentTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PcComponent>(e =>
        {
            e.HasKey(x => new { x.PcId, x.ComponentId });
            e.Property(x => x.Amount).IsRequired();

            e.HasOne(x => x.Pc)
                .WithMany(x => x.PcComponents)
                .HasForeignKey(x => x.PcId);

            e.HasOne(x => x.Component)
                .WithMany(x => x.PcComponents)
                .HasForeignKey(x => x.ComponentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ComponentType>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<ComponentManufacturer>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(50);
            e.Property(x => x.Country).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<Pc>().HasData(
            new Pc { Id = 1, Name = "Gaming Beast X", Weight = 12.5, Warranty = 36, CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0), Stock = 5 },
            new Pc { Id = 2, Name = "Office Mini Pro", Weight = 4.2, Warranty = 24, CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0), Stock = 12 },
            new Pc { Id = 3, Name = "Creator Station", Weight = 9.8, Warranty = 36, CreatedAt = new DateTime(2026, 3, 20, 10, 15, 0), Stock = 3 }
        );

        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Name = "CPU" },
            new ComponentType { Id = 2, Name = "GPU" },
            new ComponentType { Id = 3, Name = "RAM" }
        );

        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer { Id = 1, Name = "Intel", Country = "USA" },
            new ComponentManufacturer { Id = 2, Name = "NVIDIA", Country = "USA" },
            new ComponentManufacturer { Id = 3, Name = "Kingston", Country = "USA" }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component { Id = 1, Name = "Intel Core i9", Description = "Procesor Intel Core i9", ComponentManufacturerId = 1, ComponentTypeId = 1 },
            new Component { Id = 2, Name = "GeForce RTX 4080", Description = "Karta graficzna NVIDIA", ComponentManufacturerId = 2, ComponentTypeId = 2 },
            new Component { Id = 3, Name = "Kingston Fury 32GB", Description = "Pamiec RAM 32GB", ComponentManufacturerId = 3, ComponentTypeId = 3 }
        );

        modelBuilder.Entity<PcComponent>().HasData(
            new PcComponent { PcId = 1, ComponentId = 1, Amount = 1 },
            new PcComponent { PcId = 1, ComponentId = 2, Amount = 1 },
            new PcComponent { PcId = 1, ComponentId = 3, Amount = 2 },
            new PcComponent { PcId = 2, ComponentId = 1, Amount = 1 },
            new PcComponent { PcId = 3, ComponentId = 2, Amount = 1 },
            new PcComponent { PcId = 3, ComponentId = 3, Amount = 4 }
        );
    }
}
