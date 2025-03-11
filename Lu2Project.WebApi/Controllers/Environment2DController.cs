using Lu2Project.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lu2Project.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Environment2DController : ControllerBase
    {
        private readonly IEnvironmentRepository _repository;

        public Environment2DController(IEnvironmentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Environment2D>> GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Environment2D> GetById(int id)
        {
            var environment = _repository.GetById(id);
            if (environment == null)
            {
                return NotFound();
            }
            return Ok(environment);
        }

        [HttpPost]
        public ActionResult<Environment2D> Create(Environment2D environment)
        {
            _repository.Add(environment);
            return CreatedAtAction(nameof(GetById), new { id = environment.Id }, environment);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Environment2D environment)
        {
            if (id != environment.Id)
            {
                return BadRequest();
            }

            _repository.Update(environment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _repository.Delete(id);
            return NoContent();
        }
    }
}
