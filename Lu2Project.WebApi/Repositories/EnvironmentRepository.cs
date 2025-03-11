namespace Lu2Project.WebApi.Repositories
{
    public class EnvironmentRepository : IEnvironmentRepository
    {
        private static List<Environment2D> _environments = new List<Environment2D>();

        public IEnumerable<Environment2D> GetAll() => _environments;

        public Environment2D GetById(int id) => _environments.FirstOrDefault(e => e.Id == id);

        public void Add(Environment2D environment)
        {
            environment.Id = _environments.Any() ? _environments.Max(e => e.Id) + 1 : 1;
            _environments.Add(environment);
        }

        public void Update(Environment2D environment)
        {
            var existing = _environments.FirstOrDefault(e => e.Id == environment.Id);
            if (existing != null)
            {
                existing.Name = environment.Name;
                existing.MaxHeight = environment.MaxHeight;
                existing.MaxLength = environment.MaxLength;
            }
        }

        public void Delete(int id)
        {
            var environment = _environments.FirstOrDefault(e => e.Id == id);
            if (environment != null)
            {
                _environments.Remove(environment);
            }
        }
    }
}
