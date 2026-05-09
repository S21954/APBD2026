using Microsoft.AspNetCore.Mvc;
using Zad04.Data;
using Zad04.Models;

namespace Zad04.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetRooms(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool activeOnly = false)
    {
        IEnumerable<Room> rooms = InMemoryDataStore.Rooms;

        if (minCapacity.HasValue)
        {
            rooms = rooms.Where(room => room.Capacity >= minCapacity.Value);
        }

        if (hasProjector.HasValue)
        {
            rooms = rooms.Where(room => room.HasProjector == hasProjector.Value);
        }

        if (activeOnly)
        {
            rooms = rooms.Where(room => room.IsActive);
        }

        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Room> GetRoomById([FromRoute] int id)
    {
        var room = InMemoryDataStore.Rooms.FirstOrDefault(room => room.Id == id);

        if (room is null)
        {
            return NotFound();
        }

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public ActionResult<IEnumerable<Room>> GetRoomsByBuildingCode([FromRoute] string buildingCode)
    {
        var rooms = InMemoryDataStore.Rooms
            .Where(room => string.Equals(room.BuildingCode, buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }

    [HttpPost]
    public ActionResult<Room> CreateRoom([FromBody] Room room)
    {
        room.Id = InMemoryDataStore.GetNextRoomId();
        InMemoryDataStore.Rooms.Add(room);

        return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Room> UpdateRoom([FromRoute] int id, [FromBody] Room updatedRoom)
    {
        if (updatedRoom.Id != 0 && updatedRoom.Id != id)
        {
            return BadRequest("Błędne Room Id.");
        }

        var existingRoom = InMemoryDataStore.Rooms.FirstOrDefault(room => room.Id == id);

        if (existingRoom is null)
        {
            return NotFound();
        }

        existingRoom.Name = updatedRoom.Name;
        existingRoom.BuildingCode = updatedRoom.BuildingCode;
        existingRoom.Floor = updatedRoom.Floor;
        existingRoom.Capacity = updatedRoom.Capacity;
        existingRoom.HasProjector = updatedRoom.HasProjector;
        existingRoom.IsActive = updatedRoom.IsActive;

        return Ok(existingRoom);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteRoom([FromRoute] int id)
    {
        var room = InMemoryDataStore.Rooms.FirstOrDefault(room => room.Id == id);

        if (room is null)
        {
            return NotFound();
        }

        var hasReservations = InMemoryDataStore.Reservations.Any(reservation => reservation.RoomId == id);
        if (hasReservations)
        {
            return Conflict("Nie można usunąć pokoju, który ma rezerwację.");
        }

        InMemoryDataStore.Rooms.Remove(room);
        return NoContent();
    }
}
