using System;
using DatingApp.API.Data;
using Xunit;

namespace DatingApp.API.Tests
{
    public class UserControllerTest : IClassFixture<DependencyFixture>
    {
        private readonly DependencyFixture _dependencyFixture;

        public UserControllerTest(DependencyFixture dependencyFixture)
        {
            _dependencyFixture = dependencyFixture;
        }

        [Fact]
        public void Test1()
        {
            // var c = new DefaultDbContext;

            // var r = new Data.AuthRepository();

            Assert.True(true);
        }
    }
}
