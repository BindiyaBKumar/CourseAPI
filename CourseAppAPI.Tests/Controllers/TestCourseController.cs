using CourseAppAPI.Controllers;
using CourseAppAPI.DTO;
using CourseAppAPI.Models;
using CourseAppAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            courseServiceMock.Setup(courseServiceMock => courseServiceMock.GetCourseList(It.IsAny<FilterDTO>()))
                .ReturnsAsync(expected);

            var sut = new CourseController(courseServiceMock.Object);

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
            var sut = new CourseController(courseServiceMock.Object);
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
    }
}
