using Adoption.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adoption.Models
{
    public class WishList
    {
        [Key]
        public int Id { get; set; }
        public int? DogId { get; set; }
        [ForeignKey("DogId")]
        public Dog Dog { get; set; }
        public string userId { get; set; }
        [ForeignKey("userId")]
        public ApplicationUser ApplicationUser { get; set; }
        public int Quantity { get; set; }
    }
}
