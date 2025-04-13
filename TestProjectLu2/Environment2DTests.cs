using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lu2Project.WebApi.Controllers;
using Lu2Project.WebApi.Models;
using Lu2Project.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lu2Project.WebApi.Tests
{
    [TestClass]
    public class Environment2DControllerTests
    {
        private Mock<IEnvironmentRepository> _mockRepo;
        private Environment2DController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IEnvironmentRepository>();
            _controller = new Environment2DController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetAll_ReturnsNoContent_WhenNoEnvironments()
        {
            _mockRepo.Setup(r => r.GetAll("user1"))
                     .ReturnsAsync(new List<Environment2D>());

            var result = await _controller.GetAll("user1");

            Assert.IsInstanceOfType(result.Result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task GetAll_ReturnsOk_WithEnvironments()
        {
            var environments = new List<Environment2D> { new Environment2D { Id = Guid.NewGuid(), Name = "TestEnv" } };
            _mockRepo.Setup(r => r.GetAll("user1"))
                     .ReturnsAsync(environments);

            var result = await _controller.GetAll("user1");

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedList = okResult.Value as IEnumerable<Environment2D>;
            Assert.AreEqual(1, returnedList.Count());
        }

        [TestMethod]
        public async Task GetById_ReturnsNotFound_WhenNotExists()
        {
            _mockRepo.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Environment2D)null);

            var result = await _controller.GetById(Guid.NewGuid());

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_ReturnsOk_WhenSuccessful()
        {
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.Delete(id)).ReturnsAsync(true);

            var result = await _controller.Delete(id);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task Delete_ReturnsNotFound_WhenFails()
        {
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.Delete(id)).ReturnsAsync(false);

            var result = await _controller.Delete(id);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
