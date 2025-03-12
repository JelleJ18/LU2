using Lu2Project.WebApi.Repositories;
using Lu2Project.WebApi.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi(); 

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();
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
        options.User.RequireUniqueEmail = true; 
        options.Password.RequiredLength = 8;   
        options.Password.RequireDigit = true;  
        options.Password.RequireLowercase = true; 
        options.Password.RequireUppercase = true; 
        options.Password.RequireNonAlphanumeric = true; 
    })
    .AddRoles<IdentityRole>() 
    .AddDapperStores(options =>
    {
        options.ConnectionString = sqlConnectionString; // Gebruik de connection string
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/account").MapIdentityApi<IdentityUser>();

app.MapGet("/", () => $"The API is up. Connection string found: {(sqlConnectionStringFound ? "yes" : "no")}");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers().RequireAuthorization();

app.Run();