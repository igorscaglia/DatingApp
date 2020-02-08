using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Tests
{
    public class DependencyFixture
    {
        public DependencyFixture()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<DefaultDbContext>(options => options.UseInMemoryDatabase(databaseName: "Dating"));
            //serviceCollection.AddTransient<IDepartmentRepository, DepartmentRepository>();
            //serviceCollection.AddTransient<IDepartmentAppService, DepartmentAppService>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; private set; }
    }
}