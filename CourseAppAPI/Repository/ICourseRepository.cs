using CourseAppAPI.DAL;
using CourseAppAPI.DTO;
using CourseAppAPI.Models;

namespace CourseAppAPI.Repository
{
    public interface ICourseRepository
    {
        public Task<(List<CourseDetail>,int total)> GetCourseList(FilterDTO filters);
        public Task<CourseDetail> GetCourse(int id);
        public Task<CourseDetail> AddCourse(CourseDTO courseDTO);

        public Task<CourseDetail> UpdateCourse(CourseDTO courseDTO);
        public Task DeleteCourse(int id);
    }
}
