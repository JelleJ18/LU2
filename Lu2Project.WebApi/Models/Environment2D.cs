using System.ComponentModel.DataAnnotations;

namespace Lu2Project.WebApi.Models
{
    public class Environment2D
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "The name cannot be longer than 100 characters!")]
        public string Name { get; set; }

        [Required]
        [Range(20, 200, ErrorMessage = "The length must be larger than 0 and smaller than 257.")]
        public int MaxLength { get; set; }

        [Required]
        [Range(10, 100, ErrorMessage = "The height must be larger than 0 and smaller than 257")]
        public int MaxHeight { get; set; }

        [Required]
        public string UserName { get; set; }
    }
    
}
