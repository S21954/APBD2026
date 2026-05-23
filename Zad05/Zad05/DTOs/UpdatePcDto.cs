using System.ComponentModel.DataAnnotations;

namespace Zad05.DTOs;

public class UpdatePcDto
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    public double Weight { get; set; }
    public int Warranty { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Stock { get; set; }
}
