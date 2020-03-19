using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Helpers
{
    public class UserActivityActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // next: Depois que a action foi executada
            var resultContext = await next();

            // Vamos pegar o id do usuário que está autenticado
            var userId = int.Parse(resultContext.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value);

            // Pegamos o repositório via DI
            var repo = resultContext.HttpContext.RequestServices.GetService<IDatingRepository>();

            var user = await repo.GetUser(userId);

            user.LastActive = DateTime.Now;

            await repo.SaveAll();
        }
    }
}