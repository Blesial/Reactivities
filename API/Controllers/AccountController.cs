using System.Security.Claims;
using API.Dtos;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// NO USAREMOS MEDIATOR -> TRATAREMOS IDENTITY COMO UNA PARTE SEPARADA DE NUESTRA APP .
// Es bueno tenerlo separado de nuestra logica de negocio . 
// 

// usaremos este controller para logear el usuario y cuando lo haga devolverle un TOKEN
// ESTAMOS HACIENDO LA AUTENTICACION A TRAVES DE ESTE  API CONTROLLER 
// User Management: The UserManager is used to create, update, delete, and find user accounts. 
// It abstracts the underlying data store (typically a database) and provides a high-level API for working with user data.
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        // UserManager from the Identity Framework into your AccountController is essential
        // for implementing user authentication and authorization in your application. 
        // The UserManager is a critical component of the ASP.NET Identity Framework,
        // and it provides a convenient and secure way to manage user-related functionality,
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }
        // Creamos un endpoint para el login del usuario. 
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized();

            // el metodo checkpassword compara la password del user con la password enviada y devuelve booleano
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (result)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();

        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // verificamos que el userName no exista en nuestra base de datos. (el email lo mismo pero lo hicimos dentro de la configuracion de servicios.)
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName))
            {
                return BadRequest("Username is already taken");
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                return BadRequest("Email is already taken");
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };

            // luego podemos usar nuestro userManager para realmente crear el User.

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            // luego vemos si el resultado fue exitoso:
            if (result.Succeeded)
            {
                // entonces devolveremos un nuevo UserDto
                return CreateUserObject(user);
            };

            // si no tenemos un resultado exitoso cuando creamos nuestro usuario devolveremos un bad request:

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // User Object -> claims principle for a user associated with the executing action
            // como estamos autenticando contra nuestros controllers con un token, nuestro CLAIM PRINCIPLE
            // esta basado en ESE TOKEN.
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            if (user != null)
            {
                return CreateUserObject(user);
            }


            return BadRequest();
        }


        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Image = null,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName
            };
        }

    }
}