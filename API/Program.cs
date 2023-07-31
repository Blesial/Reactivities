using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// ESTE ARCHIVO SE DIVIDE EN 2: 
// 1- AGREGAR SERVICIOS
// 2- AGREGAR MIDlewares

// Add services to the container.

// SERVICES SON PARA AGREGAR LOGICA A NUESTRO CODIGO. OSEA, MAS FUNCIONALIDADES. ESTO SE HACE
// A TRAVES DE INYECCION DE DEPENDENCIAS!!!!
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Esta línea de código registra el contexto de la base de datos DataContext en el contenedor 
// de servicios. Esto permite que otras partes de la aplicación puedan solicitar y 
//utilizar una instancia del contexto de la base de datos mediante la 
// INYECCION DE DEPENDENCIAS!!! ver video q paso diego 
builder.Services.AddDbContext<DataContext>(opt => // expresion LAMBDA (PROGRAMACION FUNCIONAL)
{
    
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// En resumen, este código establece la configuración para el contexto de la base de datos en
// la aplicación ASP.NET Core, permitiendo que la aplicación utilice Entity Framework Core para
// interactuar con una base de datos SQLite mediante la inyección de dependencias. 
//Al utilizar AddDbContext en Startup.cs, se asegura que el contexto de la base de datos 
//esté disponible en toda la aplicación y se pueda utilizar en los controladores, servicios u 
//otras partes del código que lo necesiten.

// La cadena de conexión (connection string en inglés) es una cadena de texto que especifica cómo 
// una aplicación de software se conecta a una fuente de datos, como una base de datos, 
// un servicio web o cualquier otro origen de datos.

// In Entity Framework, migration refers to the process of managing changes to the database 
// schema based on changes made to the data model in the application. It allows developers to keep 
// the database schema in sync with the changes in the domain model (entities and relationships) 
// without the need to manually create or modify the database tables.


var app = builder.Build();

// Configure the HTTP request pipeline.
// ESTO ES UN MIDDLEWARE, EL HTTP REQUEST PASA POR UN PIPELINE. 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Cuando una solicitud HTTP llega a tu aplicación (por ejemplo, cuando un cliente navega a 
// una página web), la aplicación crea un "alcance de servicio" para esa solicitud. 
// Es como si se abriera una pequeña tienda temporal dentro de la tienda principal.
// Dentro de ese alcance de servicio, el ServiceProvider es responsable de proporcionar todas las dependencias necesarias para manejar esa solicitud específica. 
using var scope = app.Services.CreateScope();

// es como un inventario, almacena todas las dependencias de la app. 
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();

    await context.Database.MigrateAsync(); // es similar al update command de la base de datos. actualiza la bd segun 
    // la migracion, o si la bd no existe, la crea
    //Las migraciones son actualizaciones en el esquema de la base de
    // datos basadas en los cambios en el modelo de datos (por ejemplo, agregar tablas o columnas).

    await Seed.SeedData(context);
}
catch (Exception ex)
{

//En lugar de depender directamente de una implementación concreta de la "bitácora", MiClase dependerá de una "abstracción" llamada ILogger. Esto es lo que significa el enfoque basado en abstracciones. La clase ILogger es como un contrato que define los métodos que MiClase necesita para registrar mensajes.
// En el contexto del marco de trabajo de registro (logging) en .NET Core, el enfoque basado en abstracciones significa que las clases que necesitan registrar mensajes no dependen directamente de un proveedor de registro específico, sino que dependen de la interfaz ILogger. La interfaz ILogger es una abstracción que define los métodos para registrar mensajes, pero no contiene la lógica real de cómo se realizan los registros. En cambio, la lógica de registro se encapsula en implementaciones concretas de la interfaz, que son los proveedores de registro.
// Con este enfoque, puedes cambiar cómo se registran los mensajes simplemente inyectando una implementación diferente de ILogger en MiClase. Por ejemplo, en la configuración de la aplicación, puedes decidir si deseas utilizar FileLogger o ConsoleLogger para registrar mensajes, y el cambio será transparente para MiClase.
// La abstracción es un principio de diseño que consiste en simplificar y reducir la complejidad de un objeto o sistema, resaltando solo los detalles relevantes para el contexto en el que se está utilizando. Es como un modelo o representación simplificada de la realidad. En programación, la abstracción implica ocultar los detalles internos de un objeto y exponer solo la información y funcionalidad esencial que otros objetos necesitan conocer.Una interfaz es una "abstracción" en el sentido de que describe un conjunto de métodos y propiedades que un objeto debe implementar para cumplir un contrato específico. Es como un contrato que define qué puede hacer un objeto, sin especificar cómo lo hace internamente
// Una interfaz es una "abstracción" en el sentido de que describe un conjunto de métodos y propiedades que un objeto debe implementar para cumplir un contrato específico. Es como un contrato que define qué puede hacer un objeto, sin especificar cómo lo hace internamente
    var logger = services.GetRequiredService<ILogger<Program>>();

    logger.LogError(ex, "An error has occurred");
}

// Una vez que la solicitud ha sido atendida y se cierra el alcance de servicio,
//  todas las dependencias resueltas dentro de ese alcance se "desechan

app.Run();
