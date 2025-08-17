using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace CourseAppAPI.Models
{
    public class CourseDetail
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        public string? CourseNumber { get; set; }

        [Required]
        public string? CourseName { get; set; }

        public double CourseDuration { get; set; }

        [MaxLength(50)]
        public string? CourseTutor { get; set; } = "";

        public double CourseCost { get; set; }

        [Required]
        [MaxLength(200)]
        public string? CourseDescription { get; set; } 

        public string CourseStatus { get; set; } = "Active";
        
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
