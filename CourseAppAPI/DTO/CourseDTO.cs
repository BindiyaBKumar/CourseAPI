using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace CourseAppAPI.DTO
{
    public class CourseDTO
    {        
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
