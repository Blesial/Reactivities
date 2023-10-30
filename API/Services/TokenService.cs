
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    // En resumen, IdentityServiceExtension configura la autenticación y TokenService genera 
    //tokens JWT basados en esa configuración, permitiendo así la autenticación y autorización de 
    // los usuarios en la aplicación.
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        // los claims son declaraciones que hace el usuario de si mismo. 
        // esta informacion van dentro del token. 
        // Usamos estos claims para establecer si el usuario es quien dice ser. y por ejemplo,
        // tambien lo compararemos con los atendees de las activities para ver si tiene el rol necesario para editar la actividad (host)
        // por el momemnto solo lo utilizaremos para autenticar con nuestra api. 
        public string CreateToken(AppUser appUser)
        // El propósito de este método es crear un token de seguridad para ese usuario.
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, appUser.UserName),
                new(ClaimTypes.NameIdentifier, appUser.Id),
                new(ClaimTypes.Email, appUser.Email),
            };
            // el metodo para la symetric security key viene del nuget package System.identitiyModel
            // SymetricKey es -> cuando encriptamos la key, la misma key que usamos para encriptar la usaremos para desencriptar 
            // Esta key permanecera en el server, no se debe compartir bajo ninguna condicion. 

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}