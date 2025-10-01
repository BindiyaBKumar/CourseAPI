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

        public async Task<(List<CourseDetail>,int)> GetCourseList(FilterDTO filters)
        {
            IQueryable<CourseDetail> query = _dbcontext.CourseDetails;

            //Apply Filters
            if(!string.IsNullOrWhiteSpace(filters.status))
            {
                query = query.Where(c => c.CourseStatus.Equals(filters.status, StringComparison.OrdinalIgnoreCase));
            }
            if(!string.IsNullOrWhiteSpace(filters.tutor))
            {
                query = query.Where(c => c.CourseTutor!=null && c.CourseTutor.Equals(filters.tutor, StringComparison.OrdinalIgnoreCase));
            }
            if(!string.IsNullOrWhiteSpace(filters.queryString))
            {
                query = query.Where(c => (c.CourseName!=null && c.CourseName.Contains(filters.queryString, StringComparison.OrdinalIgnoreCase)) ||
                                         (c.CourseDescription!=null && c.CourseDescription.Contains(filters.queryString, StringComparison.OrdinalIgnoreCase)));
            }

            //Apply Sorting
            query = filters.sort switch
            {
                "createdAt" => query.OrderBy(c => c.CreatedAt),
                "-createdAt" => query = query.OrderByDescending(c => c.CreatedAt),
                "name" => query.OrderBy(c => c.CourseName),
                "-name" => query.OrderByDescending(c => c.CourseName),
                "id" => query.OrderBy(c => c.CourseId),
                "-id" => query.OrderByDescending(c => c.CourseId),
                _ => query.OrderBy(c => c.CourseId)
            };

            //Apply Pagination and hit query in DB using ToListAsync()            
            var total = await query.CountAsync();
            var courselist = await query.Skip((filters.pageNumber-1) * filters.pageSize)
                                        .Take(filters.pageSize)
                                        .ToListAsync();

            if (courselist == null)
                return (new List<CourseDetail> { },0);
            return (courselist,total);
        }

        public async Task<CourseDetail> GetCourse(int id)
        {
            var course = await _dbcontext.CourseDetails.Where(x=>x.CourseId==id).FirstOrDefaultAsync();

            if (course == null)
                return new CourseDetail { CourseId=0};
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
                Console.WriteLine(ex.Message);
                return new CourseDetail { CourseId = 0 };
                }
        }

        public async Task<CourseDetail> UpdateCourse(CourseDTO course)
        {
            var currentcourse = await _dbcontext.CourseDetails.FindAsync(course.CourseId);

            if (currentcourse == null)
                return new CourseDetail{CourseId=0 }
            ;
            

            currentcourse.CourseNumber=course.CourseNumber;
            currentcourse.CourseName=course.CourseName;
            currentcourse.CourseTutor=course.CourseTutor;
            currentcourse.CourseDuration=course.CourseDuration;
            currentcourse.CourseCost=course.CourseCost;
            currentcourse.CourseDescription=course.CourseDescription;
            currentcourse.CourseStatus = course.CourseStatus;

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
