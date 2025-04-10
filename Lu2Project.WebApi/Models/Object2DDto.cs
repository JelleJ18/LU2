using System;
using System.ComponentModel.DataAnnotations;

namespace Lu2Project.WebApi.Models
{
    public class Object2DDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid EnvironmentId { get; set; }
        [Required]
        public string PrefabId { get; set; }
        [Required]
        public float PositionX { get; set; }
        [Required]
        public float PositionY { get; set; }
        [Required]
        public float ScaleX { get; set; }
        [Required]
        public float ScaleY { get; set; }
        [Required]
        public float RotationZ { get; set; }
    }
}