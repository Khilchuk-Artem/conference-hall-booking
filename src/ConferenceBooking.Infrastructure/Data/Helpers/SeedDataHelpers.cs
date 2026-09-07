using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Infrastructure.Data.Helpers;

public static class SeedDataHelpers
{
    private static readonly Guid ProjectorId = new("d8b8f4d4-3a6f-4f78-9f5a-7e1c9b2a4d11");
    private static readonly Guid WiFiId = new("a4c2e7b1-6d90-4b35-8f21-3c7a5e9d2b44");
    private static readonly Guid SoundId = new("f1e3b6c8-2a54-4d97-8b30-6e9c5a7d1f22");
    private static readonly Guid HallAId = new("b7e4a1c9-5d23-4f86-9a10-2c6e8d3b7f55");
    private static readonly Guid HallBId = new("c9f2d6a8-1b47-4e03-8c65-5a7d9f2b6e88");
    private static readonly Guid HallCId = new("e3a5c7f9-8d12-4b64-9e30-1a6c5d7b2f99");

    public static List<AdditionalService> LoadAdditionalServices()
    {
        return new List<AdditionalService>
        {
            new()
            {
                Id = ProjectorId,
                Name = "Projector",
                Price = 500,
                IsDeleted = false
            },
            new()
            {
                Id = WiFiId,
                Name = "Wi-Fi",
                Price = 300,
                IsDeleted = false
            },
            new()
            {
                Id = SoundId,
                Name = "Sound",
                Price = 700,
                IsDeleted = false
            }
        };
    }

    public static List<ConferenceHall> LoadConferenceHalls()
    {
        return new List<ConferenceHall>
        {
            new()
            {
                Id = HallAId,
                Name = "Hall A",
                Capacity = 50,
                RentRate = 2000,
                IsDeleted = false
            },
            new()
            {
                Id = HallBId,
                Name = "Hall B",
                Capacity = 100,
                RentRate = 3500,
                IsDeleted = false
            },
            new()
            {
                Id = HallCId,
                Name = "Hall C",
                Capacity = 30,
                RentRate = 1500,
                IsDeleted = false
            }
        };
    }

    public static List<object> LoadConferenceHallAdditionalServices()
    {
        return new List<object>
        {
            new { ConferenceHallId = HallAId, AdditionalServiceId = ProjectorId },
            new { ConferenceHallId = HallAId, AdditionalServiceId = WiFiId },
            new { ConferenceHallId = HallAId, AdditionalServiceId = SoundId },
            new { ConferenceHallId = HallBId, AdditionalServiceId = ProjectorId },
            new { ConferenceHallId = HallBId, AdditionalServiceId = WiFiId },
            new { ConferenceHallId = HallBId, AdditionalServiceId = SoundId },
            new { ConferenceHallId = HallCId, AdditionalServiceId = ProjectorId },
            new { ConferenceHallId = HallCId, AdditionalServiceId = WiFiId },
            new { ConferenceHallId = HallCId, AdditionalServiceId = SoundId }
        };
    }
}
