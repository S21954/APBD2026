namespace Zad05.Models;

public class PcComponent
{
    public int PcId { get; set; }
    public int ComponentId { get; set; }
    public int Amount { get; set; }
    public Pc Pc { get; set; } = null!;
    public Component Component { get; set; } = null!;
}
