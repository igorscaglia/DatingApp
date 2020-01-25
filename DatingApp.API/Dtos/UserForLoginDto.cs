using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForLoginDto
    {
        [Required]
        public string UserName { get; set; }    

        [Required]
        [StringLength(8 , MinimumLength = 4, ErrorMessage = "Você deve especificar uma senha entre 4 e 8 caracteres.")]
        public string Password { get; set; }   
    }
}