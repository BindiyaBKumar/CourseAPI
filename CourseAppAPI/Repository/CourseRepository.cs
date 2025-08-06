using CourseAppAPI.DAL;
using CourseAppAPI.DTO;
using CourseAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseAppAPI.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseDetailDBContext _dbcontext;
        public CourseRepository(CourseDetailDBContext dbcontext) 
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<CourseDetail>> GetCourseList()
        {
            var courselist = await _dbcontext.CourseDetails.ToListAsync();

            if (courselist == null)
                return null;
            return courselist;
        }

        public async Task<CourseDetail> GetCourse(int id)
        {
            var course = await _dbcontext.CourseDetails.Where(x=>x.CourseId==id).FirstOrDefaultAsync();

            if (course == null)
                return null;
            return course;
        }

        public async Task<CourseDetail> AddCourse(CourseDTO course)
        {
            try
            {
                CourseDetail courseDetail = new CourseDetail
                {
                    CourseName = course.CourseName,
                    CourseNumber = course.CourseNumber,
                    CourseDuration = course.CourseDuration,
                    CourseTutor = course.CourseTutor,
                    CourseCost = course.CourseCost,
                    CourseDescription = course.CourseDescription,

                };
                _dbcontext.CourseDetails.Add(courseDetail);
                await _dbcontext.SaveChangesAsync();
                return courseDetail;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<CourseDetail> UpdateCourse(CourseDTO course)
        {
            var currentcourse = await _dbcontext.CourseDetails.FindAsync(course.CourseId);

            if (currentcourse == null)
                return null;
            

            currentcourse.CourseNumber=course.CourseNumber;
            currentcourse.CourseName=course.CourseName;
            currentcourse.CourseTutor=course.CourseTutor;
            currentcourse.CourseDuration=course.CourseDuration;
            currentcourse.CourseCost=course.CourseCost;
            currentcourse.CourseDescription=course.CourseDescription;

            await _dbcontext.SaveChangesAsync();

            return currentcourse;
        }

        public async Task DeleteCourse(int id)
        {
            var course = await _dbcontext.CourseDetails.FindAsync(id);

            if (course != null)
            {
                _dbcontext.Remove(course);
                await _dbcontext.SaveChangesAsync();
            }
        }
    }
}
