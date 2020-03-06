using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Tests
{
    public class DependencyFixture
    {
        public DependencyFixture()
        {
            var serviceCollection = new ServiceCollection();

            // Add in memory ef database
            // serviceCollection.AddDbContext<DefaultDbContext>(options => options.UseInMemoryDatabase(databaseName: "Dating"));

            //Auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            serviceCollection.AddSingleton<IMapper>(mapper);

            // Build de DI
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; private set; }
    }
}