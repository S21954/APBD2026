using System.ComponentModel.DataAnnotations;

namespace Zad04.Models;

public class Reservation : IValidatableObject
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "RoomId musi być większe od 0.")]
    public int RoomId { get; set; }

    [Required]
    [MinLength(1)]
    public string OrganizerName { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string Topic { get; set; } = string.Empty;

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    [Required]
    [MinLength(1)]
    public string Status { get; set; } = "planned";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
        {
            yield return new ValidationResult(
                "EndTime musi być późniejsze niż StartTime.",
                new[] { nameof(EndTime), nameof(StartTime) });
        }
    }
}