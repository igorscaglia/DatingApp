using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Configuration;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    // O atributo ApiController força a utilização do atributo Route.
    // Ele trambém permite a validação automática por DataAnnotations nas classes de DTO
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;
        private readonly AuthConfiguration _authConfiguration;

        public AuthController(IAuthRepository repository, IOptions<AuthConfiguration> authConfiguration)
        {
            _authConfiguration = authConfiguration.Value;
            _repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userName = userForRegisterDto.UserName.ToLower();

            if (await _repository.UserExists(userName))
                return BadRequest("Usuário já está registrado.");

            var newUser = new User()
            {
                UserName = userName
            };

            var createdUser = await _repository.Register(newUser, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(UserForLogin userForLoginDto)
        {
            var userFromRepo = await _repository.Login(userForLoginDto.UserName.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            // Vamos criar as afirmações
            var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.UserName),
                };

            // A chave segura baseado em um texto de validação na configuração
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_authConfiguration.TokenValidateSecurityKey));

            // A credencial baseado na chave segura e no algoritmo de 512 bits
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Criamos a 'meta' token
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // Criamos o objeto que vai manipular a criação e serialização da token
            var tokenHandler = new JwtSecurityTokenHandler();

            // Criamos a token efetivamente
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }

        // private async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        // {
        //     // Se esse Controller não tivesse o atributo ApiController, poderiamos 
        //     // ter a mesma validação usando o FromBody no parâmetro dto e chamando
        //     // a validação pelo ModelStat
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     var userName = userForRegisterDto.UserName.ToLower();

        //     if (await _repository.UserExists(userName))
        //         return BadRequest("Usuário já está registrado.");

        //     var newUser = new User()
        //     {
        //         UserName = userName
        //     };

        //     var createdUser = await _repository.Register(newUser, userForRegisterDto.Password);

        //     return StatusCode(201);
        // }
    }
}