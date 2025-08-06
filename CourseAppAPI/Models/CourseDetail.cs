using System.ComponentModel.DataAnnotations;

namespace CourseAppAPI.Models
{
    public class CourseDetail
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        public string CourseNumber { get; set; }

        [Required]
        public string CourseName { get; set; }

        public double CourseDuration { get; set; }

        public string CourseTutor { get; set; }

        public double CourseCost { get; set; }

        public string CourseDescription { get; set; }

    }
}
