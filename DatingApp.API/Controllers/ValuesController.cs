using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.Controllers
{
    [Authorize] // Esse contralador precisa que o requisitante esteja autenticado, com exceção da decoração AllowAnonymous 
    [ApiController]
    [Route("api/[controller]")]
    [Obsolete]
    // Somente para testes. Criado com o template Core 2.2
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly SqliteDataContext _sqliteDataContext;

        public ValuesController(ILogger<ValuesController> logger, SqliteDataContext sqliteDataContext)
        {
            _sqliteDataContext = sqliteDataContext;
            _logger = logger;
        }

        // GET api/values
        // IActionResult retorna não só strings, como também respostas http, como por exemplo ok (200 Response)
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetValues()
        {
            var values = await _sqliteDataContext.Values.ToListAsync();

            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [AllowAnonymous] // Esse método não precisa que o requisitante faça autenticação antes
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await _sqliteDataContext.Values.FirstOrDefaultAsync(x => x.Id == id);

            // Se retornar nulo então retorna 204 No Content
            return Ok(value);
        }

        // POST api/values
        // Criar um valor
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        // Editar um valor
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        // Remover um valor
        [HttpDelete]
        public void Delete(int id)
        {

        }
    }
}
