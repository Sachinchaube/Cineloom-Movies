using System.ComponentModel.DataAnnotations;

namespace MovieBookingPro.DTOs
{
    public class MovieDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? PosterUrl { get; set; }
    }

    public class MovieCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Length of {0} should be between {2} and {1}")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(30)]
        public string Genre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Language is required")]
        [StringLength(30)]
        public string Language { get; set; } = string.Empty;

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 600, ErrorMessage = "Duration must be between {1} and {2} minutes")]
        public int DurationInMinutes { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Release date is required")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [StringLength(300)]
        public string? PosterUrl { get; set; }
    }

    public class MovieEditDto : MovieCreateDto
    {
        public int MovieId { get; set; }
    }
}
