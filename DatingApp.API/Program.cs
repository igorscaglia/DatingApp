using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Estratégia de seeding
            // https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Pegamos o contexto pelo DI
                    var context = services.GetRequiredService<DefaultDbContext>();
                    // Aplica um migration caso haja um pendente ou cria o banco de dados
                    context.Database.Migrate();
                    // Popula o banco de dados com a massa de dados de usuários
                    Seed.SeedUsers(context);
                }
                catch (System.Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Erro durante aplicação do migration na inicialização do programa.");
                }
            }

            // Executar o host
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>

            // Ocorre aqui a leitura do appsettings.json
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // A classe Startup.cs é usada como parâmetro de inicialização 
                    // para a construção do serviço web
                    webBuilder.UseStartup<Startup>();
                });
    }
}
