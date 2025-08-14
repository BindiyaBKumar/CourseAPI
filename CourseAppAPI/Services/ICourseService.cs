using CourseAppAPI.DTO;
using CourseAppAPI.Models;

namespace CourseAppAPI.Services
{
    public interface ICourseService
    {
        public Task<PaginatedResponse<CourseDTO>> GetCourseList(FilterDTO filters);
        public Task<CourseDTO> GetCourse(int id);
        public Task<CourseDTO> AddCourse(CourseDTO input);
        public Task<CourseDTO> UpdateCourse(CourseDTO input);
        public Task DeleteCourse(int id);
    }
}
