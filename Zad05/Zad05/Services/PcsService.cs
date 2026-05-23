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
        return await _context.Pcs
            .Select(x => new PcResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Weight = x.Weight,
                Warranty = x.Warranty,
                CreatedAt = x.CreatedAt,
                Stock = x.Stock
            })
            .ToListAsync();
    }

    public async Task<List<ComponentResponseDto>?> GetComponents(int id)
    {
        var exists = await _context.Pcs.AnyAsync(x => x.Id == id);

        if (!exists)
        {
            return null;
        }

        return await _context.PcComponents
            .Where(x => x.PcId == id)
            .Select(x => new ComponentResponseDto
            {
                Id = x.Component.Id,
                Name = x.Component.Name,
                Description = x.Component.Description,
                Manufacturer = x.Component.ComponentManufacturer.Name,
                Type = x.Component.ComponentType.Name,
                Amount = x.Amount
            })
            .ToListAsync();
    }

    public async Task<PcResponseDto> Create(CreatePcDto dto)
    {
        var pc = new Pc
        {
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
            Weight = pc.Weight,
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
}
