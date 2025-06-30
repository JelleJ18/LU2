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
    public class Environment2DControllerTests
    {
        // Test dat GetById een NotFound-resultaat geeft als de omgeving niet bestaat
        [TestMethod]
        public async Task GetById_ReturnsNotFound_WhenEnvironmentDoesNotExist()
        {
            // Arrange
            var repoMock = new Mock<IEnvironmentRepository>();
            repoMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Environment2D?)null);

            var controller = new Environment2DController(repoMock.Object);

            // Act
            var result = await controller.GetById(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        // Test dat Delete een Ok-resultaat geeft bij succesvolle verwijdering
        [TestMethod]
        public async Task Delete_ReturnsOk_WhenDeleteSucceeds()
        {
            // Arrange
            var repoMock = new Mock<IEnvironmentRepository>();
            repoMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(true);

            var controller = new Environment2DController(repoMock.Object);

            // Act
            var result = await controller.Delete(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}