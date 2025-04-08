using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lu2Project.WebApi.Models;

namespace Lu2Project.WebApi.Repositories
{
    public interface IObject2DRepository
    {
        Task<IEnumerable<Object2DDto>> GetAll();
        Task<Object2DDto> GetById(Guid id);
        Task<IEnumerable<Object2DDto>> GetByEnvironmentId(Guid environmentId);
        Task<Object2DDto> Add(Object2DDto obj);
        Task<Object2DDto> Update(Object2DDto obj);
        Task<bool> Delete(Guid id);
        Task<bool> DeleteByEnvironmentId(Guid environmentId);
    }
}
