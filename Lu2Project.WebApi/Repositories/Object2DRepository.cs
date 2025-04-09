using Lu2Project.WebApi.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Lu2Project.WebApi.Repositories
{
    public class Object2DRepository : IObject2DRepository
    {
        private readonly string _connectionString;

        public Object2DRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("SqlConnectionString");
        }

        public async Task<IEnumerable<Object2DDto>> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM [Object2D]";
            var objects = await connection.QueryAsync<Object2DDto>(query);
            return objects;
        }

        public async Task<Object2DDto> GetById(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM [Object2D] WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Object2DDto>(query, new { Id = id });
        }

        public async Task<IEnumerable<Object2DDto>> GetByEnvironmentId(Guid environmentId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM [Object2D] WHERE EnvironmentId = @EnvironmentId";
            var objects = await connection.QueryAsync<Object2DDto>(query, new { EnvironmentId = environmentId });
            return objects;
        }

        public async Task<Object2DDto> Add(Object2DDto obj)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                INSERT INTO [Object2D] (Id, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, EnvironmentId) 
                VALUES (@Id, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @EnvironmentId);
                SELECT * FROM [Object2D] WHERE Id = @Id;";

            if (obj.Id == Guid.Empty)
            {
                obj.Id = Guid.NewGuid();
            }

            return await connection.QuerySingleAsync<Object2DDto>(query, obj);
        }

        public async Task<Object2DDto> Update(Object2DDto obj)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                UPDATE [Object2D]
                SET PrefabId = @PrefabId, 
                    PositionX = @PositionX, 
                    PositionY = @PositionY, 
                    ScaleX = @ScaleX, 
                    ScaleY = @ScaleY, 
                    RotationZ = @RotationZ, 
                    EnvironmentId = @EnvironmentId
                WHERE Id = @Id;
                SELECT * FROM [Object2D] WHERE Id = @Id;";

            return await connection.QuerySingleOrDefaultAsync<Object2DDto>(query, obj);
        }

        public async Task<bool> Delete(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "DELETE FROM [Object2D] WHERE Id = @Id";
            return await connection.ExecuteAsync(query, new { Id = id }) > 0;
        }

        public async Task<bool> DeleteByEnvironmentId(Guid environmentId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "DELETE FROM [Object2D] WHERE EnvironmentId = @EnvironmentId";
            return await connection.ExecuteAsync(query, new { EnvironmentId = environmentId }) > 0;
        }
    }
}
