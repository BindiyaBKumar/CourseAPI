using CourseAppAPI.Controllers;
using CourseAppAPI.DTO;
using CourseAppAPI.Models;
using CourseAppAPI.Services;
using CourseAppAPI.Tests.TestData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAppAPI.Tests.Controllers
{
    public class TestCourseController
    {

        [Fact]
        public async Task GetCourseList_Returns200_WithPayLoad()
        {
            // Arrange
            var expected = new PaginatedResponse<CourseDTO>
            {
                Items = new List<CourseDTO>(),
                TotalItems = 0,
                PageNumber = 1,
                PageSize = 20,
                HasNextPage = false,
                HasPreviousPage = false
            };
            var courseServiceMock = new Mock<ICourseService>();            
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions();
            var cache = new MemoryCache(new MemoryCacheOptions());
            courseServiceMock.Setup(courseServiceMock => courseServiceMock.GetCourseList(It.IsAny<FilterDTO>()))
                .ReturnsAsync(expected);

            var sut = new CourseController(courseServiceMock.Object,cache, memoryCacheEntryOptions);

            // Act
            var result = await sut.GetCourseList(
                page: 1,
                pageSize: 20,
                sort: "-createdAt",
                status: null,
                tutor: null,
                q: null);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);

            var payload = Assert.IsType<PaginatedResponse<CourseDTO>>(ok.Value);
            Assert.Equal(expected.PageNumber, payload.PageNumber);
            Assert.Equal(expected.PageSize, payload.PageSize);
            Assert.Equal(expected.TotalItems, payload.TotalItems);
        }

        [Theory]
        [InlineData(0,20, "-createdAt",null,null,null)]// Invalid page
        [InlineData(1, 0, "-createdAt", null, null, null)]// Invalid page size
        [InlineData(1, 2000, "-createdAt", null, null, null)]// Invalid page size
        public async Task GetCourseList_Returns400_WhenPageOrPageSizeInvalid(int page, int pageSize, string? sort, string? status, string? tutor, string? q)
        {
            // Arrange
            var courseServiceMock = new Mock<ICourseService>();
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new CourseController(courseServiceMock.Object,cache,memoryCacheEntryOptions);
            // Act
            var result = await sut.GetCourseList(
                page: page, 
                pageSize: pageSize,
                sort: sort,
                status: status,
                tutor: tutor,
                q: q);
            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
            Assert.Equal("Page number and page size should be greater than 0 and page size should not exceed 100 !", badRequest.Value);
        }

        [Fact]
        public async Task GetCourseList_Returns400_on_Exception()
        {
            //Arrange
            var courseServiceMock = new Mock<ICourseService>();
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions();
            var cache = new MemoryCache(new MemoryCacheOptions());
            courseServiceMock.Setup(courseServiceMock => courseServiceMock.GetCourseList(It.IsAny<FilterDTO>())).Throws<Exception>();
            var sut = new CourseController(courseServiceMock.Object,cache, memoryCacheEntryOptions);

            //Act
            var exception = await sut.GetCourseList(
                page: 1,
                pageSize: 10,
                sort: "-createdAt",
                status: null,
                tutor: null,
                q: null
                );

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(exception);
        }

        [Fact]
        public async Task GetCourse_Returns200_WithPayLoad()
        {
            // Arrange
            var expected = new CourseDTO
            {
                CourseId = 1,
                CourseName = "Test Course",
                CourseNumber = "TC101",
                CourseDuration = 30,
                CourseTutor = "John Doe",
                CourseCost = 100,
                CourseDescription = "This is a test course.",
                CourseStatus = "Active",
                CreatedAt = DateTime.Now
            };
            var courseServiceMock = new Mock<ICourseService>();
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions();
            var cache = new MemoryCache(new MemoryCacheOptions());
            courseServiceMock.Setup(courseServiceMock => courseServiceMock.GetCourse(It.IsAny<int>()))
                .ReturnsAsync(expected);
            var sut = new CourseController(courseServiceMock.Object,cache,memoryCacheEntryOptions);
            // Act
            var result = await sut.GetCourse(1);
            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
            var payload = Assert.IsType<CourseDTO>(ok.Value);
            Assert.Equal(expected.CourseId, payload.CourseId);
            Assert.Equal(expected.CourseName, payload.CourseName);
        }

        [Theory]
        [InlineData(-1)] // Negative ID
        [InlineData(0)]  // Zero ID
        public async Task GetCourse_Returns400_WhenIdIsInvalid(int id)
        {
            // Arrange
            var courseServiceMock = new Mock<ICourseService>();
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new CourseController(courseServiceMock.Object,cache,memoryCacheEntryOptions);
            // Act
            var result = await sut.GetCourse(id);
            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
            Assert.Equal("Id should be greater than 0 !", badRequest.Value);
        }

        [Fact]
        public async Task GetCourse_Returns404_WhenCourseNotFound()
        {
            //Arrange
            var courseServiceMock = new Mock<ICourseService>();
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions();
            var cache = new MemoryCache(new MemoryCacheOptions());
            courseServiceMock.Setup(courseServiceMock => courseServiceMock.GetCourse(It.IsAny<int>()))
                .ReturnsAsync(new CourseDTO { CourseId = 0 }); // Simulating that the course was not found
            var sut = new CourseController(courseServiceMock.Object,cache,memoryCacheEntryOptions);

            //Act
            var result = await sut.GetCourse(1);

            //Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
            Assert.Equal("A course with id 1 is not found !", notFound.Value);
        }

        [Fact]
        public async Task CreateCourse_Returns200_WithPayload()
        {
            // Arrange
            var expected = new CourseDTO
            {
                CourseId = 1,
                CourseName = "New Course",
                CourseNumber = "NC101",
                CourseDuration = 45,
                CourseTutor = "Jane Doe",
                CourseCost = 150,
                CourseDescription = "This is a new course.",
                CourseStatus = "Active",
                CreatedAt = DateTime.Now
            };
            var courseServiceMock = new Mock<ICourseService>();
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions();
            var cache = new MemoryCache(new MemoryCacheOptions());
            courseServiceMock.Setup(courseServiceMock => courseServiceMock.AddCourse(It.IsAny<CourseDTO>()))
                .ReturnsAsync(expected);
            var sut = new CourseController(courseServiceMock.Object,cache,memoryCacheEntryOptions);
            // Act
            var result = await sut.CreateCourse(expected);
            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
            var payload = Assert.IsType<CourseDTO>(ok.Value);
            Assert.Equal(expected.CourseId, payload.CourseId);
        }

        [Theory]
        [ClassData(typeof(CourseListTestData))]
        public async Task CreateCourse_Returns400_WhenInputIsInvalid(CourseDTO input)
        {
            // Arrange
            var courseServiceMock = new Mock<ICourseService>();
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new CourseController(courseServiceMock.Object, cache, memoryCacheEntryOptions);
            sut.ModelState.AddModelError("CourseName", "Required");
            sut.ModelState.AddModelError("CourseNumber", "Required");

            // Act

            var result = await sut.CreateCourse(input);
            
            // Assert
            if (input.CourseNumber == null)
            {
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
                var problem = Assert.IsType<SerializableError>(badRequest.Value);
                Assert.True(problem.ContainsKey("CourseNumber"));
            }
            else if (input.CourseName == null)
            {
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
                var problem = Assert.IsType<SerializableError>(badRequest.Value);
                Assert.True(problem.ContainsKey("CourseName"));
            }
            
        }
    }
}
