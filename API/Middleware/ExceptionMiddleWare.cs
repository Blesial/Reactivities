using System.Net;
using System.Text.Json;
using Application.Core;

namespace API.Middleware
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next; // The RequestDelegate is a delegate representing the next middleware to invoke.
        private readonly ILogger<ExceptionMiddleWare> _logger; //  which is used for logging information about exceptions and other events in the middleware.
        private readonly IHostEnvironment _env;

        public ExceptionMiddleWare(RequestDelegate next, ILogger<ExceptionMiddleWare> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }


        // cuando nuestra app recibe un request va a buscar este metodo en nuestro middleware y es aqui donde procesamos nuestra logica. el nombre es o si ese. 
        public async Task InvokeAsync(HttpContext context)  // This method is the main entry point for the middleware
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json"; // no estamos en el contexto de un api controller, que por default lo devuelven con ese tipo. pero aqui no.
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                                                                                       // chequeamos en que contexto estamos:
                var response = _env.IsDevelopment()
                    // creates an AppException object that contains information about the error
                    ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace.ToString())
                    : new AppException(context.Response.StatusCode, "Internal Server Error");


                //This code configures the JSON serialization options to use camelCase for property names
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                //  It serializes the response object into a JSON string using the specified options: 
                var json = JsonSerializer.Serialize(response, options);
                // : Finally, it writes the JSON response to the HTTP response stream
                await context.Response.WriteAsync(json);
            }
        }
    }
}