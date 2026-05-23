namespace Zad05.DTOs;

public class ComponentResponseDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ComponentManufacturerResponseDto Manufacturer { get; set; } = null!;
    public ComponentTypeResponseDto Type { get; set; } = null!;
}
