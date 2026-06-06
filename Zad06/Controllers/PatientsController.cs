using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zad06.Data;
using Zad06.Dtos;
using Zad06.Models;

namespace Zad06.Controllers;

[ApiController]
[Route("api/patients")]
public sealed class PatientsController : ControllerBase
{
    private const string LikeEscapeCharacter = @"\";

    private readonly HospitalContext _context;

    public PatientsController(HospitalContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<PatientResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PatientResponseDto>>> GetPatients(
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        IQueryable<Patient> query = _context.Patients
            .AsNoTracking()
            .AsSplitQuery()
            .Include(patient => patient.Admissions)
                .ThenInclude(admission => admission.Ward)
            .Include(patient => patient.BedAssignments)
                .ThenInclude(assignment => assignment.Bed)
                    .ThenInclude(bed => bed.BedType)
            .Include(patient => patient.BedAssignments)
                .ThenInclude(assignment => assignment.Bed)
                    .ThenInclude(bed => bed.Room)
                        .ThenInclude(room => room.Ward);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var trimmedSearch = search.Trim();
            var pattern = $"%{EscapeLikePattern(trimmedSearch)}%";

            query = query.Where(patient =>
                EF.Functions.Like(patient.FirstName, pattern, LikeEscapeCharacter) ||
                EF.Functions.Like(patient.LastName, pattern, LikeEscapeCharacter));
        }

        var patients = await query
            .OrderBy(patient => patient.Pesel)
            .Select(patient => ToPatientResponse(patient))
            .ToListAsync(cancellationToken);

        return Ok(patients);
    }

    [HttpPost("{pesel}/bedassignments")]
    [ProducesResponseType(typeof(AssignBedResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssignBedResponseDto>> AssignBed(
        [FromRoute]
        [Required(ErrorMessage = "Parametr 'pesel' jest wymagany.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Parametr 'pesel' musi składać się dokładnie z 11 cyfr.")]
        string pesel,
        [FromBody] AssignBedRequestDto request,
        CancellationToken cancellationToken)
    {
        var from = request.From!.Value;
        var to = request.To;
        var wardName = request.Ward!.Trim();
        var bedTypeName = request.BedType!.Trim();

        await using var transaction = await _context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable,
            cancellationToken);

        var patientExists = await _context.Patients
            .AnyAsync(patient => patient.Pesel == pesel, cancellationToken);

        if (!patientExists)
        {
            return NotFound(new { message = $"Pacjent o numerze PESEL '{pesel}' nie istnieje." });
        }

        var ward = await _context.Wards
            .AsNoTracking()
            .SingleOrDefaultAsync(w => w.Name == wardName, cancellationToken);

        if (ward is null)
        {
            return NotFound(new { message = $"Oddział '{wardName}' nie istnieje." });
        }

        var bedType = await _context.BedTypes
            .AsNoTracking()
            .SingleOrDefaultAsync(bt => bt.Name == bedTypeName, cancellationToken);

        if (bedType is null)
        {
            return NotFound(new { message = $"Typ łóżka '{bedTypeName}' nie istnieje." });
        }

        var requestedEnd = to ?? DateTime.MaxValue;

        var freeBed = await _context.Beds
            .Include(bed => bed.BedType)
            .Include(bed => bed.Room)
                .ThenInclude(room => room.Ward)
            .Where(bed => bed.BedTypeId == bedType.Id && bed.Room.WardId == ward.Id)
            .Where(bed => !bed.BedAssignments.Any(assignment =>
                assignment.From < requestedEnd &&
                from < (assignment.To ?? DateTime.MaxValue)))
            .OrderBy(bed => bed.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (freeBed is null)
        {
            return NotFound(new
            {
                message = $"Brak wolnego łóżka typu '{bedTypeName}' na oddziale '{wardName}' w podanym okresie."
            });
        }

        var bedAssignment = new BedAssignment
        {
            PatientPesel = pesel,
            BedId = freeBed.Id,
            From = from,
            To = to
        };

        _context.BedAssignments.Add(bedAssignment);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        var response = new AssignBedResponseDto
        {
            Id = bedAssignment.Id,
            PatientPesel = pesel,
            From = bedAssignment.From,
            To = bedAssignment.To,
            Bed = ToBedResponse(freeBed)
        };

        return Created($"/api/patients/{pesel}/bedassignments/{bedAssignment.Id}", response);
    }

    private static PatientResponseDto ToPatientResponse(Patient patient) => new()
    {
        Pesel = patient.Pesel,
        FirstName = patient.FirstName,
        LastName = patient.LastName,
        Age = patient.Age,
        Sex = patient.Sex ? "Male" : "Female",
        Admissions = patient.Admissions
            .OrderBy(admission => admission.Id)
            .Select(ToAdmissionResponse)
            .ToList(),
        BedAssignments = patient.BedAssignments
            .OrderBy(assignment => assignment.Id)
            .Select(ToBedAssignmentResponse)
            .ToList()
    };

    private static AdmissionResponseDto ToAdmissionResponse(Admission admission) => new()
    {
        Id = admission.Id,
        AdmissionDate = admission.AdmissionDate,
        DischargeDate = admission.DischargeDate,
        Ward = ToWardResponse(admission.Ward)
    };

    private static BedAssignmentResponseDto ToBedAssignmentResponse(BedAssignment assignment) => new()
    {
        Id = assignment.Id,
        From = assignment.From,
        To = assignment.To,
        Bed = ToBedResponse(assignment.Bed)
    };

    private static BedResponseDto ToBedResponse(Bed bed) => new()
    {
        Id = bed.Id,
        BedType = ToBedTypeResponse(bed.BedType),
        Room = ToRoomResponse(bed.Room)
    };

    private static BedTypeResponseDto ToBedTypeResponse(BedType bedType) => new()
    {
        Id = bedType.Id,
        Name = bedType.Name,
        Description = bedType.Description
    };

    private static RoomResponseDto ToRoomResponse(Room room) => new()
    {
        Id = room.Id,
        HasTv = room.HasTv,
        Ward = ToWardResponse(room.Ward)
    };

    private static WardResponseDto ToWardResponse(Ward ward) => new()
    {
        Id = ward.Id,
        Name = ward.Name,
        Description = ward.Description
    };

    private static string EscapeLikePattern(string value) => value
        .Replace(LikeEscapeCharacter, LikeEscapeCharacter + LikeEscapeCharacter)
        .Replace("%", LikeEscapeCharacter + "%")
        .Replace("_", LikeEscapeCharacter + "_");
}
