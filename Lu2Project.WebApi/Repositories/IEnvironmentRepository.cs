using Lu2Project.WebApi.Models;

namespace Lu2Project.WebApi.Repositories
{
    public interface IEnvironmentRepository
    {
        Task<List<Environment2D>> GetAll(string UserName);
        Task<Environment2D> GetById(Guid id);
        Task<Environment2D> Add(Environment2D environment);
        Task<Environment2D> Update(Environment2D environment);
        Task<bool> Delete(Guid id);
    }
}
