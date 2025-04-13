using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Lu2Project.WebApi.Models;
using Lu2Project.WebApi.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Lu2Project.WebApi.Tests
{
    public class EnvironmentRepositoryTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<EnvironmentRepository>> _mockLogger;
        private readonly EnvironmentRepository _repository;

        public EnvironmentRepositoryTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<EnvironmentRepository>>();

            _mockConfiguration.Setup(c => c.GetValue<string>("SqlConnectionString"))
                .Returns("FakeConnectionString");

            _repository = new EnvironmentRepository(_mockConfiguration.Object, _mockLogger.Object);
        }

        public async Task GetAll_ShouldReturnList()
        {
            var result = await _repository.GetAll("testUser");
            Assert.NotNull(result);
        }

        public async Task Add_ShouldInsertAndReturnEnvironment()
        {
            var env = new Environment2D
            {
                Name = "TestEnv",
                MaxLength = 10,
                MaxHeight = 10,
                UserName = "tester"
            };

            var result = await _repository.Add(env);

            Assert.NotNull(result);
            Assert.Equal(env.Name, result.Name);
        }
    }
}
