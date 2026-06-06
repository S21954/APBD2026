namespace Zad06.Dtos;

public sealed class PatientResponseDto
{
    public string Pesel { get; init; } = null!;

    public string FirstName { get; init; } = null!;

    public string LastName { get; init; } = null!;

    public int Age { get; init; }

    public string Sex { get; init; } = null!;

    public List<AdmissionResponseDto> Admissions { get; init; } = [];

    public List<BedAssignmentResponseDto> BedAssignments { get; init; } = [];
}

public sealed class AdmissionResponseDto
{
    public int Id { get; init; }

    public DateTime AdmissionDate { get; init; }

    public DateTime? DischargeDate { get; init; }

    public WardResponseDto Ward { get; init; } = null!;
}

public sealed class BedAssignmentResponseDto
{
    public int Id { get; init; }

    public DateTime From { get; init; }

    public DateTime? To { get; init; }

    public BedResponseDto Bed { get; init; } = null!;
}

public sealed class BedResponseDto
{
    public int Id { get; init; }

    public BedTypeResponseDto BedType { get; init; } = null!;

    public RoomResponseDto Room { get; init; } = null!;
}

public sealed class BedTypeResponseDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string Description { get; init; } = null!;
}

public sealed class RoomResponseDto
{
    public string Id { get; init; } = null!;

    public bool HasTv { get; init; }

    public WardResponseDto Ward { get; init; } = null!;
}

public sealed class WardResponseDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string Description { get; init; } = null!;
}
