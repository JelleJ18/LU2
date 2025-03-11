using Lu2Project.WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IEnvironmentRepository, EnvironmentRepository>();

var sqlConnectionString= builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);
Console.WriteLine($"SqlConnectionString: {sqlConnectionString}");
Console.WriteLine($"SqlConnectionString found: {sqlConnectionStringFound}");

var app = builder.Build();

app.MapGet("/", () => $"The API is up. Connection string found: {(sqlConnectionStringFound ? "yes" : "no")}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
