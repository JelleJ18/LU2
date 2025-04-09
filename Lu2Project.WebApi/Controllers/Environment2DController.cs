using Lu2Project.WebApi.Repositories;
using Lu2Project.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lu2Project.WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class Environment2DController : ControllerBase
    {
        private readonly IEnvironmentRepository _repository;

        public Environment2DController(IEnvironmentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("userworlds")]
        public async Task<ActionResult<IEnumerable<Environment2D>>> GetAll([FromQuery]string UserName)
        {
            var environments = await _repository.GetAll(UserName);
            if (!environments.Any())
            {
                return NoContent();
            }
            return Ok(environments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Environment2D>> GetById(Guid id)
        {
            var environment = await _repository.GetById(id);
            if (environment == null)
            {
                return NotFound();
            }
            return Ok(environment);
        }

        [HttpPost]
        public async Task<ActionResult<Environment2D>> Add(Environment2D environment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdEnvironment = await _repository.Add(environment);
            return CreatedAtAction(nameof(GetById), new { id = createdEnvironment.Id }, createdEnvironment);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Environment2D>> Update(Guid id, Environment2D environment)
        {
            if (id != environment.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _repository.Update(environment);
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _repository.Delete(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }




        [HttpPost("save-with-objects")]
        public async Task<ActionResult<Environment2D>> SaveWithObjects(EnvironmentWithObjectsDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var environment = data.Environment;
            Environment2D savedEnvironment;

            if (environment.Id != Guid.Empty)
            {
                savedEnvironment = await _repository.Update(environment);
                if (savedEnvironment == null)
                {
                    return NotFound("Environment not found");
                }
            }
            else
            {
                savedEnvironment = await _repository.Add(environment);
            }

            var objectRepository = HttpContext.RequestServices.GetRequiredService<IObject2DRepository>();

            await objectRepository.DeleteByEnvironmentId(savedEnvironment.Id);

            foreach (var obj in data.Objects)
            {
                obj.EnvironmentId = savedEnvironment.Id;
                await objectRepository.Add(obj);
            }

            return Ok(new
            {
                Environment = savedEnvironment,
                ObjectCount = data.Objects.Count
            });
        }
    }
}
