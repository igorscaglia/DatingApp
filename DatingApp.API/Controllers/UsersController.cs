using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(UserActivityActionFilter))]
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _datingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IDatingRepository datingRepository, IMapper mapper,
            ILogger<UsersController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _datingRepository = datingRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _datingRepository.GetUsers();

            var usersResult = _mapper.Map<IEnumerable<UserForList>>(users);

            return Ok(usersResult);
        }

        [HttpGet("{id}", Name = nameof(GetUser))]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _datingRepository.GetUser(id);

            // O AutoMapper faz a transferência para o DTO através de profiles - AutoMapperProfiles
            var userResult = _mapper.Map<UserForDetailed>(user);

            return Ok(userResult);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UserForUpdate userForUpdate)
        {
            // Verificamos se o usuário que está efetuando a edição é o mesmo que está autenticado na API
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            // Recuperamos o usuário
            var user = await _datingRepository.GetUser(id);

            // If it's null then there isn't a user with the param id
            if (user == null)
            {
                return NotFound(new { Message = $"User with id {id} not found." });
            }
            else
            {
                // Atualizamos o usuário recuperado com as novas informações
                _mapper.Map(userForUpdate, user);

                // Save it on database
                if (await _datingRepository.SaveAll())
                {
                    return CreatedAtRoute(nameof(GetUser), new { id = user.Id }, "User updated.");
                }
                else
                {
                    string errorMsg = "Failed updating user on server";
                    _logger.LogError(errorMsg);
                    throw new Exception(errorMsg);
                }
            }
        }

    }
}