using CourseAppAPI.DTO;
using CourseAppAPI.Models;
using CourseAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prometheus;

namespace CourseAppAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        Counter counter = Metrics.CreateCounter("my_counter", "Counter for GetCourseList Calls");
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        [Route("Hi")]
        public ActionResult<string> Hi()
        {
            return "Hello from CourseController!";
        }

        [Authorize]
        [HttpGet]        
        [Route("GetCourseList")]
        public async Task<ActionResult> GetCourseList(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sort = "-createdAt",
            [FromQuery] string? status = null,
            [FromQuery] string? tutor = null,
            [FromQuery] string? q = null)
        {
            counter.Inc();
            try
            {
                FilterDTO filters = new FilterDTO()
                {
                    pageNumber = page,
                    pageSize = pageSize,
                    sort = sort,
                    status = status,
                    tutor = tutor,
                    queryString = q

                };

                const int maxSize = 100;

                if (page<1 || pageSize<1 || pageSize>maxSize)
                    return BadRequest($"Page number and page size should be greater than 0 and page size should not exceed {maxSize} !");

                var courseList = await _courseService.GetCourseList(filters);

                if (courseList == null || courseList.Items == null)
                    return NotFound();
                return Ok(courseList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetCourse/{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            try
            {
                if (id <=0)
                    return BadRequest("Id should be greater than 0 !");

                var course = await _courseService.GetCourse(id);
                if (course.CourseId==0 || course==null)
                    return NotFound();
                return Ok(course);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("AddCourse")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseDTO input)
        {
            try
            {
                if (input == null)
                    return BadRequest("Invalid input!");
                

                var newcourse = await _courseService.AddCourse(input);
                if (newcourse==null || newcourse.CourseId==0)
                    return NotFound("Unable to add course. Please try again or contact admin.");
                return Ok(newcourse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseDTO input)
        {
            try
            {
                if (input == null)
                    return BadRequest("Invalid input!");
                if (input.CourseId <= 0)
                    return BadRequest("Please enter a valid CourseId.");

                var updatedcourse = await _courseService.UpdateCourse(input);
                if (updatedcourse == null)
                    return NotFound("Unable to find course. Please try again or contact admin.");
                return Ok(updatedcourse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                if(id<=0)
                    return BadRequest("Please enter a valid CourseId.");

                await _courseService.DeleteCourse(id);

                return Ok("The course has been deleted!");


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
