using System.ComponentModel.DataAnnotations;

namespace Lu2Project.WebApi.Repositories
{
    public class Environment2D
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 1)]
        public string Name { get; set; }

        [Range(20, 500)]
        public int MaxHeight { get; set; }

        [Range(20, 500)]
        public int MaxLength { get; set; }
    }
}
