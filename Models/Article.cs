﻿namespace PojistakNET.Models
{
    public class Article
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
