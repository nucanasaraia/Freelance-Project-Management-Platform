using FluentAssertions;
using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Data;
using Microsoft.EntityFrameworkCore;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Moq;
using System.Net;

namespace Freelance.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IUserLoggerService> _mockLogger;

        public AuthServiceTests()
        {
            _mockEmailService = new Mock<IEmailService>();
            _mockTokenService = new Mock<ITokenService>();
            _mockLogger = new Mock<IUserLoggerService>();
        }

        private AuthService CreateService(List<User> users)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new DataContext(options);
            context.Users.AddRange(users);
            context.SaveChanges();

            return new AuthService(context, _mockEmailService.Object,
                _mockTokenService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Registration_ShouldReturnConflict_WhenEmailAlreadyExists()
        {
            // Arrange
            var existingUsers = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "john",
                    Email = "john@test.com",
                    PasswordHash = "hash"
                }
            };

            var service = CreateService(existingUsers);

            var request = new RegistrationRequest
            {
                Username = "newuser",
                Email = "john@test.com",
                Password = "password123"
            };

            // Act
            var result = await service.Registration(request);

            // Assert
            result.Status.Should().Be(HttpStatusCode.Conflict);
            result.Message.Should().Be("Email already exists");
        }

        [Fact]
        public async Task Registration_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var service = CreateService(new List<User>());

            _mockEmailService
                .Setup(x => x.SendVerificationCode(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(ApiResponseFactory.Success("sent"));

            var request = new RegistrationRequest
            {
                Username = "newuser",
                Email = "newuser@test.com",
                Password = "password123"
            };

            // Act
            var result = await service.Registration(request);

            // Assert
            result.Status.Should().Be(HttpStatusCode.OK);
            result.Message.Should().Be("Registration successful. Verify email.");
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenUserNotFound()
        {
            // Arrange
            var service = CreateService(new List<User>());

            var request = new LogInRequest
            {
                Username = "nonexistent",
                Password = "password123"
            };

            // Act
            var result = await service.LogIn(request);

            // Assert
            result.Status.Should().Be(HttpStatusCode.Unauthorized);
            result.Message.Should().Be("Invalid credentials");
        }
    }
}