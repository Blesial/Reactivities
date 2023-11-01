using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        // necesitamos acceso al contexto http , y como no estamos dentro del contexto del API (INFRASTRUCTURE ESTA SEPARADO DE LA ARQUITECTURA HEXAGONAL, AL IGUAL QUE NUESTRA PERSISTENCE LAYER)
        // POR ESO ES QUE USAMOS ESTA INTERFAZ QUE NOS DA EL ACCESO.
        // este httpcontexto contiene nuestro USER OBJECT, y con el podemos acceder A LOS CLAIMS, DENTRO DEL TOKEN. 
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            
        }
        public string GetUser()
        // mira lo que podemos acceder gracias a haber inyectado el httpContext.
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}