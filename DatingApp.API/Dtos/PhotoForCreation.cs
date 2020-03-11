using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Dtos
{
    public class PhotoForCreation
    {
        public IFormFile File { get; set; }

        public string Description { get; set; }
    }
}