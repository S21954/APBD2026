using System.ComponentModel.DataAnnotations;

namespace Zad04.DTOs;

public class CreateRoomDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }
}