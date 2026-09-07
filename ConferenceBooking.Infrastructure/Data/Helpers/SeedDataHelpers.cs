using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Infrastructure.Data.Helpers;

public static class SeedDataHelpers
{
    private static readonly Guid ProjectorId = new("0ecf09ee-9673-4a65-a00c-47af18394fdc");
    private static readonly Guid WiFiId = new("13e371d2-5852-4449-9cd4-c2e3ea26d4b7");
    private static readonly Guid SoundId = new("9e59d0dd-1045-493a-832d-5573bc549bf2");
    private static readonly Guid HallAId = new("f7b30b25-dda7-402e-b103-5074f8d607cc");
    private static readonly Guid HallBId = new("9b0b9e7a-33bd-402e-99cf-1128e0232378");
    private static readonly Guid HallCId = new("2f25e9a3-94b3-446c-b40d-4a7f5c26708c");

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
