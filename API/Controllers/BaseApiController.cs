using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]


    // BaseApiController está diseñada para proporcionar a los controladores descendientes una forma conveniente de acceder al 
    // servicio IMediator para enviar solicitudes y manejar eventos utilizando el patrón Mediator. 
    //Esto simplifica el código de los controladores descendientes y promueve un diseño limpio y desacoplado.
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;


        // para obtener una instancia de IMediator a través del mecanismo de inyección de dependencias de ASP.NET Core.
        // Esto significa que la primera vez que se accede a Mediator, se crea una instancia de IMediator y se almacena en _mediator: 
        protected IMediator Mediator => _mediator ??=
         HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandleResult<T>(Result<T> result)
        {   

            if(result == null) return NotFound();

            if (result.IsSuccess && result.Value != null) return Ok(result.Value);  

            if (result.IsSuccess && result.Value == null) return NotFound();

                return BadRequest(result.Error);
        }
    }

    // Prop Mediator:  es una propiedad CALCULADA (propiedad de solo lectura) llamada Mediator.
    //  Esta propiedad permite a los controladores descendientes acceder al servicio IMediator de manera sencilla.
}