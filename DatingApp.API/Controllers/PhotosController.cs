using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CloudinaryConfiguration = DatingApp.API.Configuration.CloudinaryConfiguration;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/users/{userId:int}/photos")]
    public class PhotosController : ControllerBase
    {
        private readonly ILogger<PhotosController> _logger;
        private readonly IMapper _mapper;
        private readonly IDatingRepository _datingRepository;
        private readonly Cloudinary _cloudinary;

        public PhotosController(
            IDatingRepository datingRepository,
            IMapper mapper,
            ILogger<PhotosController> logger, 
            IOptions<CloudinaryConfiguration> cloudinaryConfiguration
        )
        {
            _logger = logger;
            _mapper = mapper;
            _datingRepository = datingRepository;

            Account account = new Account(
                cloudinaryConfiguration.Value.CloudName,
                cloudinaryConfiguration.Value.ApiKey,
                cloudinaryConfiguration.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _datingRepository.GetPhoto(id);

            var photoDetailed = _mapper.Map<PhotoForDetailed>(photoFromRepo);

            return Ok(photoDetailed);
        }

        [HttpPost] // A rota desse action está no topo
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreation photoForCreation)
        {
            // Verificamos se o usuário que está efetuando a edição é o mesmo que está autenticado na API
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var file = photoForCreation.File;
            var uploadResult = new ImageUploadResult();

            // Se houver um arquivo para upload
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),

                        // Vamos aproveitar e fazer uma transformação na foto
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    // Executar o upload no Cloudinary
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            else
            {
                BadRequest("Photo is empty.");
            }

            var userFromRepo = await _datingRepository.GetUser(userId);

            // Se houver um erro no Cloudinary vamos propagar ele
            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            var photo = _mapper.Map<Photo>(photoForCreation);

            photo.IsMain = !userFromRepo.Photos.Any(p => p.IsMain);
            photo.PublicId = uploadResult.PublicId;
            photo.Url = uploadResult.Uri.ToString();
            photo.DateAdded = DateTime.Now;

            userFromRepo.Photos.Add(photo);

            // Salvar a photo no banco
            if (await _datingRepository.SaveAll())
            {
                // Mapeamos para não passar o usuário da FK também!
                var photoDetailed = _mapper.Map<PhotoForDetailed>(photo);
                
                // AtAction não precisamos dar o nome para a rota (AtRoute) (tá no escopo)
                // Passamos a foto também
                return CreatedAtAction(nameof(GetPhoto), new { id = photo.Id, userId = userId }, photoDetailed);
            }
            else
            {
                string errorMsg = "Failed adding photo on server.";
                _logger.LogError(errorMsg);
                throw new Exception(errorMsg);
            }
        }
    }
}
