namespace Zad05.Models;

public class PcComponent
{
    public int PcId { get; set; }
    public string ComponentCode { get; set; } = string.Empty;
    public int Amount { get; set; }
    public Pc Pc { get; set; } = null!;
    public Component Component { get; set; } = null!;
}
