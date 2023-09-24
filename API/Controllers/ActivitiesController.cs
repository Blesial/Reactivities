using Microsoft.AspNetCore.Mvc;
using Domain;
using Application.Activities;

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


        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            // Send es un metodo de mediator. No confundirse con List que es la clase dentro de activities
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            return await Mediator.Send(new Details.Query
            {

                Id = id
            }
        );
        }

        [HttpPost]
        // IACTIONRESULT NOS DA ACCESO A LOS METODOS DEL RESPONSE COMO EL OK Y ESOS. SIN NECESIDAD DE ESPECIFICAR QUE TIPO DE DATO TRAE EL ACTION RESULT
        // YA QUE EN ESTE CASO NO DEVUELVE NADA UN POST. 
        public async Task<IActionResult> CreateActivity([FromBody]Activity activity)
        {
            await Mediator.Send(new Creates.Command {Activity = activity});

            return Ok();
        }
    }
}


// So the flow is:
// WHEN AN HTTP REQUEST COMES IN , AND OUR PROGRAM CLASS KNOW WHERE IT NEEDS TO GO, ITS GOING TO
// SEND IT TO THE ACTIVITIES CONTROLLER, SO ITS GONNA CREATE A NEW INSTANCE OF THIS CONTROLLER, AND SAY:
// AHA! YOU NEED A DATACONTEXT SO ITS GOING TO CREATE AN INSTANCE OF THE DATACONTXT AND THEN ITS GOING TO BE 
// AVAILABLE INSIDE OF THIS CLASS.  