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
using DatingApp.API.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using DatingApp.API.Helpers;
using Pomelo.EntityFrameworkCore.MySql.Storage;

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
        // Aqui também, de forma comparativa, configura a camada do Middleware
        public void ConfigureServices(IServiceCollection services)
        {
            // Para injeção da configuração personalizada
            // Temos que usar o IOptions lá no controlador
            services.AddOptions();
            var authConfigurationSection = Configuration.GetSection("AuthConfiguration");

            services.Configure<AuthConfiguration>(authConfigurationSection);
            var authConfiguration = authConfigurationSection.Get<AuthConfiguration>();

            // Para injeção dos contextos de banco de dados
            // services.AddDbContext<DefaultDbContext>(x => x.UseSqlite(this.Configuration.GetConnectionString("SqliteConnectionString")));
            services.AddDbContext<DefaultDbContext>(x => x.UseMySql(this.Configuration.GetConnectionString("MySQLConnectionString"),
            options =>
            {
                options.ServerVersion(new ServerVersion(new Version(8, 0, 19)));
            }));

            // No AddScoped o objeto é criado 1 vez a cada request
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddControllers();
            services.AddCors();

            // Vamos adicionar o mecanismo de validação de autenticação que queremos utilizar.
            // É compatível com o método de autenticação que criamos, evidente.
            // Sem esse validação, o atributo Authorize funciona mesmo assim, contudo 
            // a mensagem para o consumidor da API não será inteligível
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(authConfiguration.TokenValidateSecurityKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. 
        // Use this method to configure the HTTP request pipeline.
        // De forma comparativa, representa e configura a camada do Middleware
        // A ordem das chamadas dos métodos aqui importam!
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
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    // Se houver uma exceção não tratada na aplicação vamos 
                    // escrever ela direto no response do pipeline
                    builder.Run(async httpContext =>
                    {
                        // Forçamos o erro 500 para a resposta
                        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = httpContext.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            // Nós adicionamos aqui o CORS manualmente pois estamos manipulando
                            // o pipeline manualmente e o app.UseCors não funciona aqui
                            httpContext.Response.AddApplicationError(error.Error.Message);

                            // Escrevemos o erro que a aplicação pegou no response
                            await httpContext.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            // Não vamos utilizar https portanto não vamos usar essa chamada
            // app.UseHttpsRedirection();

            // No Core 2.2 Era app.UseMvc() no lugar de routing e endpoints
            app.UseRouting();

            // Essa chamada tem que ficar antes do UseAuthorization
            app.UseAuthentication();

            app.UseAuthorization();

            // Tem que ficar depois do UseRounting e antes do UseEndpoints
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
