using Application.Activities;
using Application.Core;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{

    //  Esta clase nos va a permitir reducir el codigo de nuestra clase de entrada en Program.cs
    // estamos extendiendo el metodo Services para poder hacer cleancode.

    // static xq no necesitamos ni queremos crear una instancia de nuestra clase. 

    // 
    public static class ApplicationServiceExtensions
    {
        // especificamos que vamos a devolver (iservicecollection).
        // dspm pasamos los parametros : el primero de cualquier extension method es la cosa que vamos a extender...
        // aqui vamos a extender this iservicecollection. el this se refiere a -> Services
        // el 2do es configuracion. 
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            // version mediator 12 para agregar el servicio:
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(List.Handler).Assembly));

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // usamos el assembly de mappingprofiles para localizar todos los perfiles de mapeo que usamos dentro de nuestro proyecto
            // al igual que mediator, cuando la app inicia, automapper se registra como un servicio, va a mirar dentro del assembly mappingprofiles
            // y registrar todos los perfiles, asi se pueden usar cuando nos lo cruzamos en nuestro codigo  
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            // Esta línea de código registra el contexto de la base de datos DataContext en el contenedor 
            // de servicios. Esto permite que otras partes de la aplicación puedan solicitar y 
            //utilizar una instancia del contexto de la base de datos mediante la    
            // INYECCION DE DEPENDENCIAS!!! ver video q paso diego
            // hay que hacer dotnet restore para que aparezca el metodo AddDbContext. 
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Creates>();

            services.AddDbContext<DataContext>(opt => // expresion LAMBDA (PROGRAMACION FUNCIONAL)
            {

                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });


            // En resumen, este código establece la configuración para el contexto de la base de datos en
            // la aplicación ASP.NET Core, permitiendo que la aplicación utilice Entity Framework Core para
            // interactuar con una base de datos SQLite mediante la inyección de dependencias. 
            //Al utilizar AddDbContext  , se asegura que el contexto de la base de datos 
            //esté disponible en toda la aplicación y se pueda utilizar en los controladores, servicios u 
            //otras partes del código que lo necesiten.

            // La cadena de conexión (connection string en inglés) es una cadena de texto que especifica cómo 
            // una aplicación de software se conecta a una fuente de datos, como una base de datos, 
            // un servicio web o cualquier otro origen de datos.

            // In Entity Framework, migration refers to the process of managing changes to the database 
            // schema based on changes made to the data model in the application. It allows developers to keep 
            // the database schema in sync with the changes in the domain model (entities and relationships) 
            // without the need to manually create or modify the database tables.

            return services;
        }
    }
}