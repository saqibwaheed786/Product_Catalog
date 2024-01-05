using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Models
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Genre { get; set; } = string.Empty;

        [Required]
        [Range(1000, 9999)] // Assuming a reasonable range for publication years
        public int PublicationYear { get; set; }

        public int ViewCount { get; set; }

        [Required]
        [StringLength(10000)]
        public string ContentText { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(DVD|CD|book|other)$", ErrorMessage = "Invalid article type")]
        public string ArticleType { get; set; } = string.Empty;
    }

}
