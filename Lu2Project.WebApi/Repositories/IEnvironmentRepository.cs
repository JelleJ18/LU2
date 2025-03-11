namespace Lu2Project.WebApi.Repositories
{
    public interface IEnvironmentRepository
    {
        IEnumerable<Environment2D> GetAll();
        Environment2D GetById(int id);
        void Add(Environment2D environment);
        void Update(Environment2D environment);
        void Delete(int id);
    }
}
