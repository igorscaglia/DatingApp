using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Controllers;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using DatingApp.API.Dtos;

namespace DatingApp.API.Tests
{
    public class UserControllerTest : IClassFixture<DependencyFixture>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        public UserControllerTest(DependencyFixture dependencyFixture)
        {
            _mapper = dependencyFixture.ServiceProvider.GetService<IMapper>();
            _logger = Mock.Of<ILogger<UsersController>>();
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateUser_ReturnsUserUpdatedAndChanged(int id)
        {
            // 1A
            var mockRepo = new Mock<IDatingRepository>();

            var userFromRepo = new User()
            {
                Id = id,
                Introduction = "Intro1",
                LookingFor = "Loking1",
                Interests = "Interest1",
                City = "Sampa1",
                Country = "Brazil1",
            };

            var userToUpdate = new UserForUpdate()
            {
                Introduction = "Intro2",
                LookingFor = "Loking2",
                Interests = "Interest2",
                City = "Sampa2",
                Country = "Brazil2",
            };

            mockRepo.Setup(repo => repo.GetUser(id))
                    .Returns(Task.FromResult<User>(userFromRepo))
                    .Verifiable();

            mockRepo.Setup(repo => repo.SaveAll())
                    .Returns(Task.FromResult<bool>(true))
                    .Verifiable();

            var controller = new UsersController(mockRepo.Object, _mapper, _logger)
                .WithIdentity(id.ToString(), "wherever");

            // 2A
            var result = await controller.Put(id, userToUpdate);

            // 3A
            mockRepo.Verify();

            var okResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal("User updated.", returnValue);

            var okResult2 = Assert.IsType<OkObjectResult>(await controller.GetUser(id));
            var updatedUser = Assert.IsType<UserForDetailed>(okResult2.Value);
            Assert.Equal(userToUpdate.Introduction, updatedUser.Introduction);
            Assert.Equal(userToUpdate.LookingFor, updatedUser.LookingFor);
            Assert.Equal(userToUpdate.Interests, updatedUser.Interests);
            Assert.Equal(userToUpdate.City, updatedUser.City);
            Assert.Equal(userToUpdate.Country, updatedUser.Country);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateNonExistentUser_ReturnsNotFound(int id)
        {
            // 1A
            var mockRepo = new Mock<IDatingRepository>();

            mockRepo.Setup(repo => repo.GetUser(id))
                    .Returns(Task.FromResult<User>(null))
                    .Verifiable();

            var userToUpdate = new UserForUpdate()
            {
                Introduction = "Intro2",
                LookingFor = "Loking2",
                Interests = "Interest2",
                City = "Sampa2",
                Country = "Brazil2",
            };

            UsersController controller = new UsersController(mockRepo.Object, _mapper, _logger)
                .WithIdentity(id.ToString(), "wherever");

            // 2A
            var result = await controller.Put(id, userToUpdate);

            // 3A
            mockRepo.Verify();

            var okResult = Assert.IsType<NotFoundObjectResult>(result);
            dynamic returnValue = okResult.Value;
            Assert.Contains($"User with id {id} not found.", returnValue.ToString());
        }
    }
}
