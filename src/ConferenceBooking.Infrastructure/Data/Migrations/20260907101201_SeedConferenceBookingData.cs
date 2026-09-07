using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConferenceBooking.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedConferenceBookingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AdditionalServices",
                columns: new[] { "Id", "IsDeleted", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("a4c2e7b1-6d90-4b35-8f21-3c7a5e9d2b44"), false, "Wi-Fi", 300m },
                    { new Guid("d8b8f4d4-3a6f-4f78-9f5a-7e1c9b2a4d11"), false, "Projector", 500m },
                    { new Guid("f1e3b6c8-2a54-4d97-8b30-6e9c5a7d1f22"), false, "Sound", 700m }
                });

            migrationBuilder.InsertData(
                table: "ConferenceHalls",
                columns: new[] { "Id", "Capacity", "IsDeleted", "Name", "RentRate" },
                values: new object[,]
                {
                    { new Guid("b7e4a1c9-5d23-4f86-9a10-2c6e8d3b7f55"), 50, false, "Hall A", 2000m },
                    { new Guid("c9f2d6a8-1b47-4e03-8c65-5a7d9f2b6e88"), 100, false, "Hall B", 3500m },
                    { new Guid("e3a5c7f9-8d12-4b64-9e30-1a6c5d7b2f99"), 30, false, "Hall C", 1500m }
                });

            migrationBuilder.InsertData(
                table: "ConferenceHallAdditionalServices",
                columns: new[] { "AdditionalServiceId", "ConferenceHallId" },
                values: new object[,]
                {
                    { new Guid("a4c2e7b1-6d90-4b35-8f21-3c7a5e9d2b44"), new Guid("b7e4a1c9-5d23-4f86-9a10-2c6e8d3b7f55") },
                    { new Guid("d8b8f4d4-3a6f-4f78-9f5a-7e1c9b2a4d11"), new Guid("b7e4a1c9-5d23-4f86-9a10-2c6e8d3b7f55") },
                    { new Guid("f1e3b6c8-2a54-4d97-8b30-6e9c5a7d1f22"), new Guid("b7e4a1c9-5d23-4f86-9a10-2c6e8d3b7f55") },
                    { new Guid("a4c2e7b1-6d90-4b35-8f21-3c7a5e9d2b44"), new Guid("c9f2d6a8-1b47-4e03-8c65-5a7d9f2b6e88") },
                    { new Guid("d8b8f4d4-3a6f-4f78-9f5a-7e1c9b2a4d11"), new Guid("c9f2d6a8-1b47-4e03-8c65-5a7d9f2b6e88") },
                    { new Guid("f1e3b6c8-2a54-4d97-8b30-6e9c5a7d1f22"), new Guid("c9f2d6a8-1b47-4e03-8c65-5a7d9f2b6e88") },
                    { new Guid("a4c2e7b1-6d90-4b35-8f21-3c7a5e9d2b44"), new Guid("e3a5c7f9-8d12-4b64-9e30-1a6c5d7b2f99") },
                    { new Guid("d8b8f4d4-3a6f-4f78-9f5a-7e1c9b2a4d11"), new Guid("e3a5c7f9-8d12-4b64-9e30-1a6c5d7b2f99") },
                    { new Guid("f1e3b6c8-2a54-4d97-8b30-6e9c5a7d1f22"), new Guid("e3a5c7f9-8d12-4b64-9e30-1a6c5d7b2f99") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConferenceHallAdditionalServices",
                keyColumns: new[] { "AdditionalServiceId", "ConferenceHallId" },
                keyValues: new object[] { new Guid("a4c2e7b1-6d90-4b35-8f21-3c7a5e9d2b44"), new Guid("b7e4a1c9-5d23-4f86-9a10-2c6e8d3b7f55") });

            migrationBuilder.DeleteData(
                table: "ConferenceHallAdditionalServices",
                keyColumns: new[] { "AdditionalServiceId", "ConferenceHallId" },
                keyValues: new object[] { new Guid("d8b8f4d4-3a6f-4f78-9f5a-7e1c9b2a4d11"), new Guid("b7e4a1c9-5d23-4f86-9a10-2c6e8d3b7f55") });

            migrationBuilder.DeleteData(
                table: "ConferenceHallAdditionalServices",
                keyColumns: new[] { "AdditionalServiceId", "ConferenceHallId" },
                keyValues: new object[] { new Guid("f1e3b6c8-2a54-4d97-8b30-6e9c5a7d1f22"), new Guid("b7e4a1c9-5d23-4f86-9a10-2c6e8d3b7f55") });

            migrationBuilder.DeleteData(
                table: "ConferenceHallAdditionalServices",
                keyColumns: new[] { "AdditionalServiceId", "ConferenceHallId" },
                keyValues: new object[] { new Guid("a4c2e7b1-6d90-4b35-8f21-3c7a5e9d2b44"), new Guid("c9f2d6a8-1b47-4e03-8c65-5a7d9f2b6e88") });

            migrationBuilder.DeleteData(
                table: "ConferenceHallAdditionalServices",
                keyColumns: new[] { "AdditionalServiceId", "ConferenceHallId" },
                keyValues: new object[] { new Guid("d8b8f4d4-3a6f-4f78-9f5a-7e1c9b2a4d11"), new Guid("c9f2d6a8-1b47-4e03-8c65-5a7d9f2b6e88") });

            migrationBuilder.DeleteData(
                table: "ConferenceHallAdditionalServices",
                keyColumns: new[] { "AdditionalServiceId", "ConferenceHallId" },
                keyValues: new object[] { new Guid("f1e3b6c8-2a54-4d97-8b30-6e9c5a7d1f22"), new Guid("c9f2d6a8-1b47-4e03-8c65-5a7d9f2b6e88") });

            migrationBuilder.DeleteData(
                table: "ConferenceHallAdditionalServices",
                keyColumns: new[] { "AdditionalServiceId", "ConferenceHallId" },
                keyValues: new object[] { new Guid("a4c2e7b1-6d90-4b35-8f21-3c7a5e9d2b44"), new Guid("e3a5c7f9-8d12-4b64-9e30-1a6c5d7b2f99") });

            migrationBuilder.DeleteData(
                table: "ConferenceHallAdditionalServices",
                keyColumns: new[] { "AdditionalServiceId", "ConferenceHallId" },
                keyValues: new object[] { new Guid("d8b8f4d4-3a6f-4f78-9f5a-7e1c9b2a4d11"), new Guid("e3a5c7f9-8d12-4b64-9e30-1a6c5d7b2f99") });

            migrationBuilder.DeleteData(
                table: "ConferenceHallAdditionalServices",
                keyColumns: new[] { "AdditionalServiceId", "ConferenceHallId" },
                keyValues: new object[] { new Guid("f1e3b6c8-2a54-4d97-8b30-6e9c5a7d1f22"), new Guid("e3a5c7f9-8d12-4b64-9e30-1a6c5d7b2f99") });

            migrationBuilder.DeleteData(
                table: "AdditionalServices",
                keyColumn: "Id",
                keyValue: new Guid("a4c2e7b1-6d90-4b35-8f21-3c7a5e9d2b44"));

            migrationBuilder.DeleteData(
                table: "AdditionalServices",
                keyColumn: "Id",
                keyValue: new Guid("d8b8f4d4-3a6f-4f78-9f5a-7e1c9b2a4d11"));

            migrationBuilder.DeleteData(
                table: "AdditionalServices",
                keyColumn: "Id",
                keyValue: new Guid("f1e3b6c8-2a54-4d97-8b30-6e9c5a7d1f22"));

            migrationBuilder.DeleteData(
                table: "ConferenceHalls",
                keyColumn: "Id",
                keyValue: new Guid("b7e4a1c9-5d23-4f86-9a10-2c6e8d3b7f55"));

            migrationBuilder.DeleteData(
                table: "ConferenceHalls",
                keyColumn: "Id",
                keyValue: new Guid("c9f2d6a8-1b47-4e03-8c65-5a7d9f2b6e88"));

            migrationBuilder.DeleteData(
                table: "ConferenceHalls",
                keyColumn: "Id",
                keyValue: new Guid("e3a5c7f9-8d12-4b64-9e30-1a6c5d7b2f99"));
        }
    }
}
