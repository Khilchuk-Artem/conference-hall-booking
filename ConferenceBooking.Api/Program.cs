using ConferenceBooking.Api;
using ConferenceBooking.Api.Idempotency;
using ConferenceBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ConferenceBookingDbContext>();
    db.Database.Migrate();
}

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<IdempotencyMiddleware>();
app.MapControllers();

app.Run();

public partial class Program;
