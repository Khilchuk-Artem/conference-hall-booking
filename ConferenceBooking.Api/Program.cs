using ConferenceBooking.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

app.UseStatusCodePages();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program;
