namespace ConferenceBooking.Api.Idempotency;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public sealed class IdempotencyAttribute : Attribute
{
}
