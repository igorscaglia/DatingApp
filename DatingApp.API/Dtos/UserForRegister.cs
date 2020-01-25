
// Usamos Dtos quando não podemos usar, por exemplo, um model diretamente quando 
// exposto a alguma interface consumidora
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    // Nessa abastração faz sentido adicionarmos a responsabilidade de validação
    public class UserForRegisterDto
    {
        [Required]
        public string UserName { get; set; }    

        [Required]
        [StringLength(8 , MinimumLength = 4, ErrorMessage = "Você deve especificar uma senha entre 4 e 8 caracteres.")]
        public string Password { get; set; }
    }
}