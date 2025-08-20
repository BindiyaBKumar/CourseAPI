using CourseAppAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAppAPI.Tests.TestData
{
    public class CourseListTestData : IEnumerable<object[]>    
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            //null course number
            yield return new object[] { new CourseDTO { CourseId = 1, CourseName = "test", CourseCost = 10, CourseDescription = "test", CourseDuration = 10, CourseNumber = null, CourseStatus = "Active", CourseTutor = "test" } }; //
            //null course name
            yield return new object[] { new CourseDTO { CourseId = 1, CourseName = null, CourseCost = 10, CourseDescription = "test", CourseDuration = 10, CourseNumber = "test", CourseStatus = "Active", CourseTutor = "test" } }; //

        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
