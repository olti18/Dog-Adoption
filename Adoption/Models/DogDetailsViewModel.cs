﻿namespace Adoption.Models
{
    public class DogDetailsViewModel
    {
        public Dog Dog { get; set; }
        public List<Comment> Comments { get; set; }
        public Comment NewComment { get; set; }
    }
}