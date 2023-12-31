using API.Extensions;
using API.Middleware;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;


// this host the app in a server
var builder = WebApplication.CreateBuilder(args);

// ESTE ARCHIVO SE DIVIDE EN 2: 
// 1- AGREGAR SERVICIOS
// 2- AGREGAR MIDlewares

// Add services to the container.

// SERVICES SON PARA AGREGAR LOGICA A NUESTRO CODIGO. OSEA, MAS FUNCIONALIDADES. ESTO SE HACE
// A TRAVES DE INYECCION DE DEPENDENCIAS!!!!
builder.Services.AddControllers(opt => {
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddCors(opt => {
    opt.AddPolicy("CorsPolicy", policy =>  
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
    });
});

builder.Services.AddIdentityService(builder.Configuration);

var app = builder.Build(); 

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleWare>();   
// ESTO ES UN MIDDLEWARE, EL HTTP REQUEST PASA POR UN PIPELINE. 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");

// asegurar que authentication este antes. primero hay que ver si el usuario es valido
// y dsp ver si tiene autorizacion para ingresar a alguna parte de la web o lo q sea. 
app.UseAuthentication();
app.UseAuthorization();

// Registra los endpoints de nuestros controllers para saber donde mandar los requests que llegan
app.MapControllers();

// Cuando una solicitud HTTP llega a tu aplicación (por ejemplo, cuando un cliente navega a 
// una página web), la aplicación crea un "alcance de servicio" para esa solicitud. (un scope)
// Es como si se abriera una pequeña tienda temporal dentro de la tienda principal.
// Dentro de ese alcance de servicio, el ServiceProvider es responsable de proporcionar todas las dependencias necesarias para manejar esa solicitud específica.

// Aca necesitamos acceder a un servicio que no se obtiene por inyeccion de dependencias como los demas de arriba x eso se hace asi "manual":
using var scope = app.Services.CreateScope();
// es como un inventario, almacena todas las dependencias de la app. 
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();

    await context.Database.MigrateAsync(); // es similar al update command de la base de datos. actualiza la bd segun 
    // la migracion, o si la bd no existe, la crea

    await Seed.SeedData(context, userManager);
}
catch (Exception ex)
{

    //En lugar de depender directamente de una implementación concreta de la "bitácora", MiClase dependerá de una "abstracción" llamada ILogger. Esto es lo que significa el enfoque basado en abstracciones. La clase ILogger es como un contrato que define los métodos que MiClase necesita para registrar mensajes.
    // En el contexto del marco de trabajo de registro (logging) en .NET Core, el enfoque basado en abstracciones significa que las clases que necesitan registrar mensajes no dependen directamente de un proveedor de registro específico, sino que dependen de la interfaz ILogger. La interfaz ILogger es una abstracción que define los métodos para registrar mensajes, pero no contiene la lógica real de cómo se realizan los registros. En cambio, la lógica de registro se encapsula en implementaciones concretas de la interfaz, que son los proveedores de registro.
    // Con este enfoque, puedes cambiar cómo se registran los mensajes simplemente inyectando una implementación diferente de ILogger en MiClase. Por ejemplo, en la configuración de la aplicación, puedes decidir si deseas utilizar FileLogger o ConsoleLogger para registrar mensajes, y el cambio será transparente para MiClase.
    // La abstracción es un principio de diseño que consiste en simplificar y reducir la complejidad de un objeto o sistema, resaltando solo los detalles relevantes para el contexto en el que se está utilizando. Es como un modelo o representación simplificada de la realidad. En programación, la abstracción implica ocultar los detalles internos de un objeto y exponer solo la información y funcionalidad esencial que otros objetos necesitan conocer.Una interfaz es una "abstracción" en el sentido de que describe un conjunto de métodos y propiedades que un objeto debe implementar para cumplir un contrato específico. Es como un contrato que define qué puede hacer un objeto, sin especificar cómo lo hace internamente
    // Una interfaz es una "abstracción" en el sentido de que describe un conjunto de métodos y propiedades que un objeto debe implementar para cumplir un contrato específico. Es como un contrato que define qué puede hacer un objeto, sin especificar cómo lo hace internamente
    var logger = services.GetRequiredService<ILogger<Program>>();

    logger.LogError(ex, "An error has occurred during migration");
}

// Una vez que la solicitud ha sido atendida y se cierra el alcance de servicio,
//  todas las dependencias resueltas dentro de ese alcance se "desechan

app.Run();
