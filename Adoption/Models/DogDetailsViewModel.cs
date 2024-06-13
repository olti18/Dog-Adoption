namespace Adoption.Models
{
    public class DogDetailsViewModel
    {
        public int DogId { get; set; } 
        public Dog Dog { get; set; }
        public List<Comment> Comments { get; set; }
        public Comment NewComment { get; set; }
      
    }
}
