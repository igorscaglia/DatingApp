using System.Collections.Generic;
using System.IO;
using System.Linq;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public static void SeedUsers(DefaultDbContext context)
        {
            // Local function igual ao AuthController somente para criar senha nos usuários de teste
            void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
            }

            // Se não tiver nenhum usuário
            if (!context.Users.Any())
            {

                // Ler todo o conteúdo do arquivo
                var userData = File.ReadAllText("Data/UserSeedData.json");

                // Converter o conteúdo do arquivo de texto para objetos do tipo User
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    byte[] passHash, passSalt;
                    CreatePasswordHash("1234", out passHash, out passSalt);
                    user.PasswordHash = passHash;
                    user.PasswordSalt = passSalt;

                    user.UserName = user.UserName.ToLower();
                    context.Users.Add(user);
                }

                context.SaveChanges();
            }
        }
    }
}