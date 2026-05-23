namespace Zad05.Models;

public class Component
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ComponentManufacturerId { get; set; }
    public int ComponentTypeId { get; set; }
    public ComponentManufacturer ComponentManufacturer { get; set; } = null!;
    public ComponentType ComponentType { get; set; } = null!;
    public ICollection<PcComponent> PcComponents { get; set; } = new List<PcComponent>();
}
