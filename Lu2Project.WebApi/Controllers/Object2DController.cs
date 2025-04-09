using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lu2Project.WebApi.Models;
using Lu2Project.WebApi.Repositories;

namespace Lu2Project.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class Object2DController : ControllerBase
    {
        private readonly IObject2DRepository _repository;

        public Object2DController(IObject2DRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Object2DDto>>> GetAll()
        {
            var objects = await _repository.GetAll();
            if (!objects.Any())
            {
                return NoContent();
            }
            return Ok(objects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Object2DDto>> GetById(Guid id)
        {
            var obj = await _repository.GetById(id);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpGet("environment/{environmentId}")]
        public async Task<ActionResult<IEnumerable<Object2DDto>>> GetByEnvironmentId(Guid environmentId)
        {
            if (environmentId == Guid.Empty)
            {
                return BadRequest("Ongeldige wereld GUID");
            }

            var objects = await _repository.GetByEnvironmentId(environmentId);
            if (!objects.Any())
            {
                return NotFound();
            }
            return Ok(objects);
        }

        [HttpPost]
        public async Task<ActionResult<Object2DDto>> Add(Object2DDto obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdObject = await _repository.Add(obj);
            return CreatedAtAction(nameof(GetById), new { id = createdObject.Id }, createdObject);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Object2DDto>> Update(Guid id, Object2DDto obj)
        {
            if (id != obj.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _repository.Update(obj);
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
    }
}