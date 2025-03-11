using Lu2Project.WebApi.Repositories;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi(); // OpenAPI (Swagger)
builder.Services.AddScoped<IEnvironmentRepository, EnvironmentRepository>();

// Controleer of de SQL Connection String is gevonden
var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);
Console.WriteLine($"SqlConnectionString: {sqlConnectionString}");
Console.WriteLine($"SqlConnectionString found: {sqlConnectionStringFound}");

// Configureer Identity
builder.Services.AddAuthorization();
builder.Services
    .AddIdentityApiEndpoints<IdentityUser>(options =>
    {
        options.User.RequireUniqueEmail = true; // Vereist een uniek e-mailadres
        options.Password.RequiredLength = 10;   // Minimale wachtwoordlengte
        options.Password.RequireDigit = true;   // Vereist een cijfer
        options.Password.RequireLowercase = true; // Vereist een kleine letter
        options.Password.RequireUppercase = true; // Vereist een hoofdletter
        options.Password.RequireNonAlphanumeric = true; // Vereist een speciaal teken
    })
    .AddRoles<IdentityRole>() // Voeg ondersteuning voor rollen toe
    .AddDapperStores(options =>
    {
        options.ConnectionString = builder.Configuration.GetConnectionString("DapperIdentity"); // Gebruik de connection string
    });

var app = builder.Build();

// Configureer de HTTP-request pipeline
app.UseHttpsRedirection();
app.UseAuthorization();

// Map Identity API endpoints
app.MapGroup("/account").MapIdentityApi<IdentityUser>();

// Homepage-route
app.MapGet("/", () => $"The API is up. Connection string found: {(sqlConnectionStringFound ? "yes" : "no")}");

// OpenAPI (Swagger) alleen in development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Vereis autorisatie voor alle controllers
app.MapControllers().RequireAuthorization();

app.Run();