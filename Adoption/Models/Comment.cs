using Adoption.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Adoption.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int DogId { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("DogId")]
        public virtual Dog Dog { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
