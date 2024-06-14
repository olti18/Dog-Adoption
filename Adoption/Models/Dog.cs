using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AdoptionContext.Models;


namespace Adoption.Models
{
    public class Dog
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Gender Gender { get; set; } //Enums
        public int Age { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public double Height { get; set; }
        [Required]
        public string ImageUrl { get; set; }


        //All Foreign KEYS
        [ForeignKey("Breed")]
        public int BreedId { get; set; }
        public Breed Breeds { get; set; }
        //
        public virtual ICollection<Comment> Comments { get; set; }
        public ICollection<AdoptionRequest> AdoptionRequests { get; set; }
    }
}
