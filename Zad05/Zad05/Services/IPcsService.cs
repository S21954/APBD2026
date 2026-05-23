using Zad05.DTOs;

namespace Zad05.Services;

public interface IPcsService
{
    Task<List<PcResponseDto>> GetAll();
    Task<List<ComponentResponseDto>?> GetComponents(int id);
    Task<PcResponseDto> Create(CreatePcDto dto);
    Task<bool> Update(int id, UpdatePcDto dto);
    Task<bool> Delete(int id);
}
