namespace ConferenceBooking.Infrastructure.Idempotency;

public sealed class IdempotencyKey
{
    public Guid Id { get; set; }
    public string Operation { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public byte[] RequestHash { get; set; } = [];
    public int? ResponseStatusCode { get; set; }
    public string? ResponseContentType { get; set; }
    public byte[] ResponseBody { get; set; } = [];
    public string? Location { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}
