namespace Zad05.Models;

public class Pc
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Weight { get; set; }
    public int Warranty { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Stock { get; set; }
    public ICollection<PcComponent> PcComponents { get; set; } = new List<PcComponent>();
}
