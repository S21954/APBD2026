namespace Zad06.Dtos;

public sealed class AssignBedResponseDto
{
    public int Id { get; init; }

    public string PatientPesel { get; init; } = null!;

    public DateTime From { get; init; }

    public DateTime? To { get; init; }

    public BedResponseDto Bed { get; init; } = null!;
}
