using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Lu2Project.WebApi.Controllers;
using Lu2Project.WebApi.Repositories;
using Lu2Project.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lu2Project.WebApi.Tests
{
    [TestClass]
    public class Object2DControllerTests
    {
        // Test dat GetByEnvironmentId een BadRequest teruggeeft wanneer de environmentId leeg is.
        [TestMethod]
        public async Task GetByEnvironmentId_ReturnsBadRequest_WhenGuidEmpty()
        {
            // Arrange
            var repoMock = new Mock<IObject2DRepository>();

            var controller = new Object2DController(repoMock.Object);

            // Act
            var result = await controller.GetByEnvironmentId(Guid.Empty);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // Test dat Delete een NotFound-resultaat teruggeeft als het object niet verwijderd kon worden.
        [TestMethod]
        public async Task Delete_ReturnsNotFound_WhenObjectNotDeleted()
        {
            // Arrange
            var repoMock = new Mock<IObject2DRepository>();
            repoMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(false);

            var controller = new Object2DController(repoMock.Object);

            // Act
            var result = await controller.Delete(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}