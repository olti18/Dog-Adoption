using System.ComponentModel.DataAnnotations;

namespace Adoption.Models
{
    public class Breed
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Breed name cannot exeed the length of 50 characters")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
