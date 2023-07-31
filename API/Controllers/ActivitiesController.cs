using Persistence;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    // deriven from controller base!!! 
    // already has the API attributtes
    public class ActivitiesController : BaseApiController
    {
        // DEPENDENCY INJECTION IN THE CONSTRUCTOR OF THE CLASS
        // In the constructor of ActivitiesController, dependency injection is used to inject an instance of DataContext into the controller. Dependency injection is a design pattern commonly used in ASP.NET Core to manage dependencies between classes. In this case, DataContext is injected into the controller to enable interaction with the database.
        //CONSTRUCTOR:

        // Private Fields: 
        private readonly DataContext _context;
        public ActivitiesController(DataContext context)
        {
            _context = context;
            
        }
        // ENDPOINTS: 

        [HttpGet]
        // return a Task because is an async. type of actionresult. and the thing thats going back in 
        //the response body its gonna be a lista of activity:
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await _context.Activities.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            return await _context.Activities.FindAsync(id);        }
        
    }
}


// So the flow is:
// WHEN AN HTTP REQUEST COMES IN , AND OUR PROGRAM CLASS KNOW WHERE IT NEEDS TO GO, ITS GOING TO
// SEND IT TO THE ACTIVITIES CONTROLLER, SO ITS GONNA CREATE A NEW INSTANCE OF THIS CONTROLLER, AND SAY:
// AHA! YOU NEED A DATACONTEXT SO ITS GOING TO CREATE AN INSTANCE OF THE DATACONTXT AND THEN ITS GOING TO BE 
// AVAILABLE INSIDE OF THIS CLASS.  