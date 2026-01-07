using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSearchAPI.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ExternalId { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Authors { get; set; } // Comma separated

        public int? FirstPublishYear { get; set; }

        [MaxLength(1000)]
        public string? CoverUrl { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
