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
using Microsoft.OpenApi.Models;
using AutoMapper;

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
            services.AddScoped<IDatingRepository, DatingRepository>();

            // Adicionar o AutoMapper - IMapper
            services.AddAutoMapper(typeof(DatingRepository).Assembly);

            // Adicionar NewtonsoftJson features
            services.AddControllers().AddNewtonsoftJson(options => {
                // Vamos ignorar o erro abaixo 
                // Newtonsoft.Json.JsonSerializationException: Self referencing loop detected for property 'user' with type 'DatingApp.API.Models.User'. Path 'photos[0]'.
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // Permitir CORS
            services.AddCors();

            // Vamos adicionar o mecanismo de validação de autenticação que queremos utilizar.
            // É compatível com o método de autenticação que criamos, evidente.
            // Sem esse validação, o atributo Authorize funciona mesmo assim, contudo 
            // a mensagem para o consumidor da API não será inteligível
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
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

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dating API", Version = "v1" });

                // Abaixo para o swagger tratar a nossa token

                //First we define the security scheme
                c.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    }
                );

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer", //The name of the previously defined security scheme.
                                    Type = ReferenceType.SecurityScheme
                                }
                            }, new List<string>()
                        }
                    }
                );

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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dating API V1");

                // To serve the Swagger UI at the app's root (http://localhost:<port>/)
                c.RoutePrefix = string.Empty;
            });


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
