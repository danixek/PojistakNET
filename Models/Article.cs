using System.ComponentModel.DataAnnotations;

namespace PojistakNET.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Název je povinný.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Obsah je povinný.")]
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
