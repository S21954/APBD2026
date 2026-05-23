using Microsoft.EntityFrameworkCore;
using Zad05.Data;
using Zad05.DTOs;
using Zad05.Models;

namespace Zad05.Services;

public class PcsService : IPcsService
{
    private readonly AppDbContext _context;

    public PcsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PcResponseDto>> GetAll()
    {
        var pcs = await _context.Pcs
            .OrderBy(x => x.Id)
            .ToListAsync();

        return pcs
            .Select(x => new PcResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Weight = Math.Round(x.Weight, 1),
                Warranty = x.Warranty,
                CreatedAt = x.CreatedAt,
                Stock = x.Stock
            })
            .ToList();
    }

    public async Task<PcDetailsResponseDto?> GetById(int id)
    {
        var pc = await _context.Pcs
            .Include(x => x.PcComponents)
                .ThenInclude(x => x.Component)
                .ThenInclude(x => x.ComponentManufacturer)
            .Include(x => x.PcComponents)
                .ThenInclude(x => x.Component)
                .ThenInclude(x => x.ComponentType)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (pc == null)
        {
            return null;
        }

        return new PcDetailsResponseDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = Math.Round(pc.Weight, 1),
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock,
            Components = pc.PcComponents
                .OrderBy(x => x.ComponentCode)
                .Select(pcComponent => new PcComponentResponseDto
                {
                    Amount = pcComponent.Amount,
                    Component = new ComponentResponseDto
                    {
                        Code = pcComponent.Component.Code,
                        Name = pcComponent.Component.Name,
                        Description = pcComponent.Component.Description,
                        Manufacturer = new ComponentManufacturerResponseDto
                        {
                            Id = pcComponent.Component.ComponentManufacturer.Id,
                            Abbreviation = pcComponent.Component.ComponentManufacturer.Abbreviation,
                            FullName = pcComponent.Component.ComponentManufacturer.FullName,
                            FoundationDate = pcComponent.Component.ComponentManufacturer.FoundationDate
                        },
                        Type = new ComponentTypeResponseDto
                        {
                            Id = pcComponent.Component.ComponentType.Id,
                            Abbreviation = pcComponent.Component.ComponentType.Abbreviation,
                            Name = pcComponent.Component.ComponentType.Name
                        }
                    }
                })
                .ToList()
        };
    }

    public async Task<PcResponseDto> Create(CreatePcDto dto)
    {
        var pc = new Pc
        {
            Id = await GetNextPcId(),
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _context.Pcs.Add(pc);
        await _context.SaveChangesAsync();

        return new PcResponseDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = Math.Round(pc.Weight, 1),
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }

    public async Task<bool> Update(int id, UpdatePcDto dto)
    {
        var pc = await _context.Pcs.FindAsync(id);

        if (pc == null)
        {
            return false;
        }

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var pc = await _context.Pcs.FindAsync(id);

        if (pc == null)
        {
            return false;
        }

        _context.Pcs.Remove(pc);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<int> GetNextPcId()
    {
        var maxId = await _context.Pcs.MaxAsync(x => (int?)x.Id);
        return (maxId ?? 0) + 1;
    }
}
