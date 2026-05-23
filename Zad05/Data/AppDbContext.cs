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
            e.ToTable("PCs");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedNever();
            e.Property(x => x.Name).IsRequired().HasMaxLength(50);
            e.Property(x => x.Weight).IsRequired().HasColumnType("float(5)");
            e.Property(x => x.Warranty).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired().HasColumnType("datetime");
            e.Property(x => x.Stock).IsRequired();
        });

        modelBuilder.Entity<Component>(e =>
        {
            e.ToTable("Components");
            e.HasKey(x => x.Code);
            e.Property(x => x.Code).IsRequired().HasColumnType("char(10)");
            e.Property(x => x.Name).IsRequired().HasMaxLength(300);
            e.Property(x => x.Description).IsRequired();
            e.Property(x => x.ComponentManufacturerId).HasColumnName("ComponentManufacturersId");
            e.Property(x => x.ComponentTypeId).HasColumnName("ComponentTypesId");

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
            e.ToTable("PCComponents");
            e.HasKey(x => new { x.PcId, x.ComponentCode });
            e.Property(x => x.PcId).HasColumnName("PCId");
            e.Property(x => x.ComponentCode).IsRequired().HasColumnType("char(10)");
            e.Property(x => x.Amount).IsRequired();

            e.HasOne(x => x.Pc)
                .WithMany(x => x.PcComponents)
                .HasForeignKey(x => x.PcId);

            e.HasOne(x => x.Component)
                .WithMany(x => x.PcComponents)
                .HasForeignKey(x => x.ComponentCode)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ComponentType>(e =>
        {
            e.ToTable("ComponentTypes");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedNever();
            e.Property(x => x.Abbreviation).IsRequired().HasMaxLength(30);
            e.Property(x => x.Name).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<ComponentManufacturer>(e =>
        {
            e.ToTable("ComponentManufacturers");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedNever();
            e.Property(x => x.Abbreviation).IsRequired().HasMaxLength(30);
            e.Property(x => x.FullName).IsRequired().HasMaxLength(300);
            e.Property(x => x.FoundationDate).IsRequired().HasColumnType("date");
        });

        modelBuilder.Entity<Pc>().HasData(
            new Pc { Id = 1, Name = "Gaming Beast X", Weight = 12.5, Warranty = 36, CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0), Stock = 5 },
            new Pc { Id = 2, Name = "Office Mini Pro", Weight = 4.2, Warranty = 24, CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0), Stock = 12 }
        );

        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Processor" },
            new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Card" },
            new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Memory" }
        );

        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer { Id = 1, Abbreviation = "AMD", FullName = "Advanced Micro Devices", FoundationDate = new DateOnly(1969, 5, 1) },
            new ComponentManufacturer { Id = 2, Abbreviation = "NV", FullName = "NVIDIA Corporation", FoundationDate = new DateOnly(1993, 4, 5) },
            new ComponentManufacturer { Id = 3, Abbreviation = "COR", FullName = "Corsair Gaming Inc.", FoundationDate = new DateOnly(1994, 1, 1) }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component { Code = "CPU0000001", Name = "Ryzen 7 7800X3D", Description = "8-core gaming processor", ComponentManufacturerId = 1, ComponentTypeId = 1 },
            new Component { Code = "GPU0000001", Name = "RTX 4080 Super", Description = "High-end gaming graphics card", ComponentManufacturerId = 2, ComponentTypeId = 2 },
            new Component { Code = "RAM0000001", Name = "Corsair Vengeance DDR5 16GB", Description = "DDR5 RAM module 16GB", ComponentManufacturerId = 3, ComponentTypeId = 3 }
        );

        modelBuilder.Entity<PcComponent>().HasData(
            new PcComponent { PcId = 1, ComponentCode = "CPU0000001", Amount = 1 },
            new PcComponent { PcId = 1, ComponentCode = "GPU0000001", Amount = 1 },
            new PcComponent { PcId = 1, ComponentCode = "RAM0000001", Amount = 2 }
        );
    }
}
