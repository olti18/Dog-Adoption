using System.ComponentModel.DataAnnotations;

namespace Adoption.Models
{
    public class AdoptionRequestViewModel
    {
        [Required]
        public int DogId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
      
    }
}
