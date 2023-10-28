using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName {get;set;}

        public string Bio {get;set;}

        // cuando creamos un usuario asi tendremos acceso a varias propiedades que vienen de la clase padre identityUser
    }
}