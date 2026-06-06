using System.ComponentModel.DataAnnotations;

namespace Zad06.Dtos;

public sealed class AssignBedRequestDto : IValidatableObject
{
    [Required(ErrorMessage = "Pole 'from' jest wymagane.")]
    public DateTime? From { get; init; }

    public DateTime? To { get; init; }

    [Required(ErrorMessage = "Pole 'bedType' jest wymagane.")]
    [StringLength(300, ErrorMessage = "Pole 'bedType' może mieć maksymalnie 300 znaków.")]
    public string? BedType { get; init; }

    [Required(ErrorMessage = "Pole 'ward' jest wymagane.")]
    [StringLength(300, ErrorMessage = "Pole 'ward' może mieć maksymalnie 300 znaków.")]
    public string? Ward { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(BedType))
        {
            yield return new ValidationResult("Pole 'bedType' nie może być puste.", [nameof(BedType)]);
        }

        if (string.IsNullOrWhiteSpace(Ward))
        {
            yield return new ValidationResult("Pole 'ward' nie może być puste.", [nameof(Ward)]);
        }

        if (From.HasValue && To.HasValue && To.Value <= From.Value)
        {
            yield return new ValidationResult("Pole 'to' musi być późniejsze niż pole 'from'.", [nameof(To)]);
        }
    }
}
