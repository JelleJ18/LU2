using Lu2Project.WebApi.Models;
using static Lu2Project.WebApi.Repositories.EnvironmentRepository;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Lu2Project.WebApi.Repositories
{
    public class EnvironmentRepository : IEnvironmentRepository
    {
            private readonly string _connectionString;

        private readonly ILogger<EnvironmentRepository> _logger;

        public EnvironmentRepository(IConfiguration configuration, ILogger<EnvironmentRepository> logger)
        {
            _connectionString = configuration.GetValue<string>("SqlConnectionString");
            _logger = logger;
        }

        public EnvironmentRepository(IConfiguration configuration)
            {
                _connectionString = configuration.GetValue<string>("SqlConnectionString");
            }

            public async Task<List<Environment2D>> GetAll(string UserName)
            {
                using var connection = new SqlConnection(_connectionString);
                var query = "SELECT * FROM Environment2D WHERE UserName = @UserName";
                var environments = await connection.QueryAsync<Environment2D>(query, new { UserName });
                return environments.ToList();
            }

            public async Task<Environment2D> GetById(Guid id)
            {
                using var connection = new SqlConnection(_connectionString);
                var query = "SELECT * FROM Environment2D WHERE Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<Environment2D>(query, new { Id = id });
            }

        public async Task<Environment2D> Add(Environment2D environment)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();  // Open de verbinding expliciet
            var query = @"
            INSERT INTO Environment2D (Id, Name, MaxLength, MaxHeight, UserName) 
            VALUES (@Id, @Name, @MaxLength, @MaxHeight, @UserName);
            SELECT * FROM Environment2D WHERE Id = @Id;";

            environment.Id = Guid.NewGuid();

            Console.WriteLine($"Executing query: {query} with parameters: {environment.Id}, {environment.Name}, {environment.MaxLength}, {environment.MaxHeight}, {environment.UserName}");

            _logger.LogInformation($"Inserting Environment: Name={environment.Name}, MaxLength={environment.MaxLength}, MaxHeight={environment.MaxHeight}, UserName={environment.UserName}");

            // Voer de query uit en log de resultaten
            var inserted = await connection.QuerySingleAsync<Environment2D>(query, environment);

            // Log het ID van de ingevoegde omgeving
            _logger.LogInformation($"Inserted Environment ID: {inserted.Id}");

            return inserted;

        }


        public async Task<Environment2D> Update(Environment2D environment)
            {
                using var connection = new SqlConnection(_connectionString);
                var query = @"
                UPDATE Environment2D
                SET Name = @Name, MaxLength = @MaxLength, MaxHeight = @MaxHeight
                WHERE Id = @Id;
                SELECT * FROM Environment2D WHERE Id = @Id;";

                return await connection.QuerySingleOrDefaultAsync<Environment2D>(query, environment);
            }

            public async Task<bool> Delete(Guid id)
            {
                using var connection = new SqlConnection(_connectionString);
                var query = "DELETE FROM Environment2D WHERE Id = @Id";
                return await connection.ExecuteAsync(query, new { Id = id }) > 0;
            }
    }
}
