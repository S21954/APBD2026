using Microsoft.AspNetCore.Mvc;
using Zad04.Data;
using Zad04.Models;

namespace Zad04.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetReservations(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        IEnumerable<Reservation> reservations = InMemoryDataStore.Reservations;

        if (date.HasValue)
        {
            reservations = reservations.Where(reservation => reservation.Date == date.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            reservations = reservations.Where(reservation =>
                string.Equals(reservation.Status, status, StringComparison.OrdinalIgnoreCase));
        }

        if (roomId.HasValue)
        {
            reservations = reservations.Where(reservation => reservation.RoomId == roomId.Value);
        }

        return Ok(reservations);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> GetReservationById([FromRoute] int id)
    {
        var reservation = InMemoryDataStore.Reservations.FirstOrDefault(reservation => reservation.Id == id);

        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> CreateReservation([FromBody] Reservation reservation)
    {
        var roomValidationResult = ValidateRoomForReservation(reservation.RoomId);
        if (roomValidationResult is not null)
        {
            return roomValidationResult;
        }

        if (HasTimeConflict(reservation))
        {
            return Conflict("Rezerwacja na ten pokój w podanej dacie już istnieje.");
        }

        reservation.Id = InMemoryDataStore.GetNextReservationId();
        InMemoryDataStore.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Reservation> UpdateReservation([FromRoute] int id, [FromBody] Reservation updatedReservation)
    {
        if (updatedReservation.Id != 0 && updatedReservation.Id != id)
        {
            return BadRequest("Id rezerwacji nieprawidłowe.");
        }

        var existingReservation = InMemoryDataStore.Reservations.FirstOrDefault(reservation => reservation.Id == id);
        if (existingReservation is null)
        {
            return NotFound();
        }

        var roomValidationResult = ValidateRoomForReservation(updatedReservation.RoomId);
        if (roomValidationResult is not null)
        {
            return roomValidationResult;
        }

        updatedReservation.Id = id;
        if (HasTimeConflict(updatedReservation, id))
        {
            return Conflict("Rezerwacja na ten pokój w podanej dacie już istnieje.");
        }

        existingReservation.RoomId = updatedReservation.RoomId;
        existingReservation.OrganizerName = updatedReservation.OrganizerName;
        existingReservation.Topic = updatedReservation.Topic;
        existingReservation.Date = updatedReservation.Date;
        existingReservation.StartTime = updatedReservation.StartTime;
        existingReservation.EndTime = updatedReservation.EndTime;
        existingReservation.Status = updatedReservation.Status;

        return Ok(existingReservation);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteReservation([FromRoute] int id)
    {
        var reservation = InMemoryDataStore.Reservations.FirstOrDefault(reservation => reservation.Id == id);

        if (reservation is null)
        {
            return NotFound();
        }

        InMemoryDataStore.Reservations.Remove(reservation);
        return NoContent();
    }

    private static ActionResult? ValidateRoomForReservation(int roomId)
    {
        var room = InMemoryDataStore.Rooms.FirstOrDefault(room => room.Id == roomId);

        if (room is null)
        {
            return new NotFoundObjectResult("Pokój nie istnieje.");
        }

        if (!room.IsActive)
        {
            return new ConflictObjectResult("Nie można utworzyć rezerwacji do nieaktywnego pokoju.");
        }

        return null;
    }

    private static bool HasTimeConflict(Reservation reservation, int? ignoredReservationId = null)
    {
        return InMemoryDataStore.Reservations.Any(existingReservation =>
            existingReservation.Id != ignoredReservationId &&
            existingReservation.RoomId == reservation.RoomId &&
            existingReservation.Date == reservation.Date &&
            !string.Equals(existingReservation.Status, "cancelled", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(reservation.Status, "cancelled", StringComparison.OrdinalIgnoreCase) &&
            reservation.StartTime < existingReservation.EndTime &&
            reservation.EndTime > existingReservation.StartTime);
    }
}
