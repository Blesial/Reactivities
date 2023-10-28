using Microsoft.AspNetCore.Mvc;
using Domain;
using Application.Activities;
using Application;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    // deriven from controller base!!! 
    // already has the API attributtes
    public class ActivitiesController : BaseApiController
    {
        // estamos usando mediator, no necesitamos inyectar el contexto de base de datos 
        // metimos el acceso a mediator en nuestro Base controller. 
        // ENDPOINTS: 

        [HttpGet]
        // return a Task because is an async. type of actionresult. and the thing thats going back in 
        //the response body its gonna be a lista of activity:


        // with i action result no necesitamos especificar ningun type del resultado de ese metodo 

        public async Task<IActionResult> GetActivities()
        {
            // Send es un metodo de mediator. No confundirse con List que es la clase dentro de activities
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [HttpGet("{id}")]

        // El IActionResult nos permite devolver http responses envez de solo el tipo del response. 
        public async Task<IActionResult> GetActivity(Guid id)
        {
          return HandleResult(await Mediator.Send(new Details.Query{Id= id}));
        }

        [HttpPost]
        // IACTIONRESULT NOS DA ACCESO A LOS METODOS DEL RESPONSE COMO EL OK Y ESOS. SIN NECESIDAD DE ESPECIFICAR QUE TIPO DE DATO TRAE EL ACTION RESULT
        // YA QUE EN ESTE CASO NO DEVUELVE NADA UN POST. 
        public async Task<IActionResult> CreateActivity([FromBody]Activity activity)
        {
            return HandleResult(await Mediator.Send(new Creates.Command {Activity = activity}));        
        }

        // FLUENT VALIDATION: VAMOS A VALIDAR LAS PETICIONES QUE RECIBIMOS INDIRECTAMENTE EN LA API
        // INDIRECTAMENTE PORQUE EN REALIZAR VAMOS A VALIDAR DENTRO DE NUESTRA LOGICA (APPLICATION LAYER)
        // POR LO QUE RECIBIREMOS LA SOLICITUD, Y ENVIAREMOS CON MEDIATOR A NUESTRA LOGICA LO RECIBIDO PARA ALLI VALIDARLA
        // INSTALAR EN NUGGET -> FLUENT VALIDATION ASP.NET CORE PARA NUESTRA APP LAYER 
        [HttpPut("{id}")]

        public async Task<IActionResult> EditActivity(Guid id, [FromBody]Activity activity)
        {
            activity.Id = id;

            return HandleResult(await Mediator.Send(new Edit.Command {Activity = activity}));
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command {Id = id}));
        }


    }
}


// So the flow is:
// WHEN AN HTTP REQUEST COMES IN , AND OUR PROGRAM CLASS KNOW WHERE IT NEEDS TO GO, ITS GOING TO
// SEND IT TO THE ACTIVITIES CONTROLLER, SO ITS GONNA CREATE A NEW INSTANCE OF THIS CONTROLLER, AND SAY:
// AHA! YOU NEED A DATACONTEXT SO ITS GOING TO CREATE AN INSTANCE OF THE DATACONTXT AND THEN ITS GOING TO BE 
// AVAILABLE INSIDE OF THIS CLASS.  