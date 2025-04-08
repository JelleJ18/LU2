using System.Collections.Generic;

namespace Lu2Project.WebApi.Models
{
    public class EnvironmentWithObjectsDto
    {
        public Environment2D Environment { get; set; }
        public List<Object2DDto> Objects { get; set; } = new List<Object2DDto>();
    }
}