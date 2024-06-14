using Adoption.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adoption.Models
{
    public class AdoptionRequest
    {
       


        [Key]
        public int Id { get; set; }
        public int? DogId { get; set; }
        [ForeignKey("DogId")]
        public Dog Dog { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        
        public string Status { get; set; } = "Pending"; //Pending by default


    }

}
