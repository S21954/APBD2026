using Zad04.Models;

namespace Zad04.Data;

public static class InMemoryDataStore
{
    public static List<Room> Rooms { get; } = new()
    {
        new Room
        {
            Id = 1,
            Name = "Sala 101",
            BuildingCode = "A",
            Floor = 1,
            Capacity = 18,
            HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 2,
            Name = "Lab 204",
            BuildingCode = "B",
            Floor = 2,
            Capacity = 24,
            HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 3,
            Name = "Sala 305",
            BuildingCode = "A",
            Floor = 3,
            Capacity = 32,
            HasProjector = false,
            IsActive = true
        },
        new Room
        {
            Id = 4,
            Name = "Sala Konsultacyjna 12",
            BuildingCode = "C",
            Floor = 0,
            Capacity = 8,
            HasProjector = false,
            IsActive = true
        },
        new Room
        {
            Id = 5,
            Name = "Audytorium 1",
            BuildingCode = "B",
            Floor = 1,
            Capacity = 80,
            HasProjector = true,
            IsActive = false
        }
    };

    public static List<Reservation> Reservations { get; } = new()
    {
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Jan Nowak",
            Topic = "Podstawy C#",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(10, 30),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 2,
            RoomId = 2,
            OrganizerName = "Anna Kowalska",
            Topic = "Warsztaty z HTTP i REST",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(12, 30),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 3,
            RoomId = 3,
            OrganizerName = "Piotr Zielinski",
            Topic = "Konsultacje projektowe",
            Date = new DateOnly(2026, 5, 11),
            StartTime = new TimeOnly(13, 0),
            EndTime = new TimeOnly(15, 0),
            Status = "planned"
        },
        new Reservation
        {
            Id = 4,
            RoomId = 4,
            OrganizerName = "Ewa Wisniewska",
            Topic = "Spotkanie organizacyjne",
            Date = new DateOnly(2026, 5, 12),
            StartTime = new TimeOnly(8, 30),
            EndTime = new TimeOnly(9, 30),
            Status = "cancelled"
        },
        new Reservation
        {
            Id = 5,
            RoomId = 2,
            OrganizerName = "Tomasz Wójcik",
            Topic = "ASP.NET Core Web API",
            Date = new DateOnly(2026, 5, 13),
            StartTime = new TimeOnly(11, 0),
            EndTime = new TimeOnly(14, 0),
            Status = "planned"
        }
    };

    public static int GetNextRoomId()
    {
        return Rooms.Count == 0 ? 1 : Rooms.Max(room => room.Id) + 1;
    }

    public static int GetNextReservationId()
    {
        return Reservations.Count == 0 ? 1 : Reservations.Max(reservation => reservation.Id) + 1;
    }
}
