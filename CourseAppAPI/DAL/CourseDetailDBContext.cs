using CourseAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseAppAPI.DAL
{
    public class CourseDetailDBContext : DbContext
    {
        public CourseDetailDBContext(DbContextOptions options) : base(options) 
        {
        }

        public DbSet<CourseDetail> CourseDetails { get; set; }
    }
}
