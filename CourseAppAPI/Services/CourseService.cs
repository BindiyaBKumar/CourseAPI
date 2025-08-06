using CourseAppAPI.DAL;
using CourseAppAPI.DTO;
using CourseAppAPI.Models;
using CourseAppAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace CourseAppAPI.Services
{
    public class CourseService : ICourseService
    { 
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository) 
        { 
           _courseRepository = courseRepository;
        }

        public async Task<List<CourseDTO>> GetCourseList()
        {
            var courseList = await _courseRepository.GetCourseList();

            if (courseList == null)
                return null;
            return courseList.Select(c => new CourseDTO
            {
                CourseId=c.CourseId,
                CourseName=c.CourseName,
                CourseNumber=c.CourseNumber,
                CourseDuration=c.CourseDuration,
                CourseTutor=c.CourseTutor,
                CourseCost=c.CourseCost,
                CourseDescription=c.CourseDescription
            }).ToList();
        }

        public async Task<CourseDTO> GetCourse(int id)
        {
            var course = await _courseRepository.GetCourse(id);

            if (course == null)
                return null;
            return new CourseDTO
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                CourseNumber = course.CourseNumber,
                CourseDuration = course.CourseDuration,
                CourseTutor = course.CourseTutor,
                CourseCost = course.CourseCost,
                CourseDescription = course.CourseDescription
            };
        }

        public async Task<CourseDTO> AddCourse(CourseDTO input)
        {
           var newcourse = await _courseRepository.AddCourse(input);

            if (newcourse == null)
                return null;

            CourseDTO output = new CourseDTO
            {
                CourseId = newcourse.CourseId,
                CourseNumber = newcourse.CourseNumber,
                CourseName = newcourse.CourseName,
                CourseDuration = newcourse.CourseDuration,
                CourseTutor = newcourse.CourseTutor,
                CourseCost = newcourse.CourseCost,
                CourseDescription = newcourse.CourseDescription
            };

            return output;

        }

        public async Task<CourseDTO> UpdateCourse(CourseDTO input)
        {
            var updatedcourse = await _courseRepository.UpdateCourse(input);

            if (updatedcourse == null)
                return null;
            CourseDTO output = new CourseDTO
            {
                CourseId = updatedcourse.CourseId,
                CourseNumber = updatedcourse.CourseNumber,
                CourseName = updatedcourse.CourseName,
                CourseDuration = updatedcourse.CourseDuration,
                CourseTutor = updatedcourse.CourseTutor,
                CourseCost = updatedcourse.CourseCost,
                CourseDescription = updatedcourse.CourseDescription
            };

            return output;
        }

        public async Task DeleteCourse(int id)
        {            
            
            await _courseRepository.DeleteCourse(id);
            

        }
    }
}
