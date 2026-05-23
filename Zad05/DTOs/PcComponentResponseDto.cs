namespace Zad05.DTOs;

public class PcComponentResponseDto
{
    public int Amount { get; set; }
    public ComponentResponseDto Component { get; set; } = null!;
}
