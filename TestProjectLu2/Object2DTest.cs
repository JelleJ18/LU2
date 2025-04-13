using System;
using System.Collections.Generic;
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
    public class Object2DTest
    {
        private Mock<IObject2DRepository> _mockRepo;
        private Object2DController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IObject2DRepository>();
            _controller = new Object2DController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetAll_ReturnsNoContent_WhenNoObjects()
        {
            _mockRepo.Setup(r => r.GetAll()).ReturnsAsync(new List<Object2DDto>());

            var result = await _controller.GetAll();

            Assert.IsInstanceOfType(result.Result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task GetById_ReturnsNotFound_WhenNotExists()
        {
            _mockRepo.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Object2DDto)null);

            var result = await _controller.GetById(Guid.NewGuid());

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetByEnvironmentId_ReturnsBadRequest_WhenInvalidGuid()
        {
            var result = await _controller.GetByEnvironmentId(Guid.Empty);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task GetByEnvironmentId_ReturnsNotFound_WhenNoObjectsFound()
        {
            var environmentId = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetByEnvironmentId(environmentId)).ReturnsAsync(new List<Object2DDto>());

            var result = await _controller.GetByEnvironmentId(environmentId);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Update_ReturnsNotFound_WhenObjectDoesNotExist()
        {
            var id = Guid.NewGuid();
            var obj = new Object2DDto { Id = id, PrefabId = "Test" };
            _mockRepo.Setup(r => r.Update(obj)).ReturnsAsync((Object2DDto)null);

            var result = await _controller.Update(id, obj);

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
