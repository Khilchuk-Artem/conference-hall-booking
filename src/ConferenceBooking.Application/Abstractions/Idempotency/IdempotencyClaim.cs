namespace ConferenceBooking.Application.Abstractions.Idempotency;

public sealed class IdempotencyClaim
{
    public required Guid Id { get; init; }
    public required bool IsNew { get; init; }
    public required byte[] RequestHash { get; init; }
    public int? ResponseStatusCode { get; init; }
    public string? ResponseContentType { get; init; }
    public byte[] ResponseBody { get; init; } = [];
    public string? Location { get; init; }
}
