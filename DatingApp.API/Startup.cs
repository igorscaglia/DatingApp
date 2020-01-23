using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // A configuração que será injetada aqui será do appsettings.json
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Aqui é onde ocorre o Dependency Injection. Dependency injection. DI é um padrão usado para construir sistemas com baixo acoplamento.
        public void ConfigureServices(IServiceCollection services)
        {
            // Para injeção dos contextos de banco de dados
            services.AddDbContext<SqlServerDataContext>( x => x.UseSqlServer(this.Configuration.GetConnectionString("SqlServerConnectionString")));
            services.AddDbContext<SqliteDataContext>( x => x.UseSqlite(this.Configuration.GetConnectionString("SqliteConnectionString")));

            services.AddControllers();
            services.AddCors();
        }

        // This method gets called by the runtime. 
        // Use this method to configure the HTTP request pipeline.
        // De forma comparativa, configura a camada Middleware OWIN
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // O ASP.NET Core sabe se é ambiente de dev pois no appsettings.json a 
            // config ASPNETCORE_ENVIRONMENT está como Development
            // Podemos configurar como Production quando criarmos um ambiente de produção e
            // usar IsProduction, por exemplo
            if (env.IsDevelopment())
            {
                // Exibe no navegador a exceção quando ocorrer
                app.UseDeveloperExceptionPage();
            }

            // Não vamos utilizar https portanto não vamos usar essa chamada
            // app.UseHttpsRedirection();

            // No Core 2.2 Era app.UseMvc() no lugar de routing e endpoints
            app.UseRouting();

            // Tem que ficar depois do UseRounting e antes do UseAuthorization
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
