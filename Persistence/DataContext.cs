using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


// THE DBCONTEXT: ES UNA COMBINACION DE REPOSITORY PATTERN Y UNIDADES DE TRABAJO ALGO ASI

// El patrón de diseño comúnmente utilizado en el desarrollo de software para abstraer y 
// encapsular la capa de acceso a datos de una aplicación es el "Patrón de Repositorio".
//  Proporciona una forma de centralizar y gestionar las operaciones de acceso a datos mediante
//   la creación de una capa de objetos (repositorios) que actúan como mediadores entre la lógica 
//   de negocios de la aplicación y el almacenamiento de datos subyacente (por ejemplo, bases de datos).


// Repository Pattern Overview:
// The pattern aims to separate the data access logic from the rest of the application's business logic.
// It provides a clear and standardized API for performing CRUD operations (Create, Read, Update, Delete) on entities.
// The application's business logic interacts with repositories instead of directly accessing the DbContext or the data storage.


// DATA CONTEXT VA A CREAR DB SETS! QUE REPRESENTAN LAS TABLAS Q VAMOS A CREAR
// BASICAMENTE ESTAMOS METIENDO ENTITIY FRAMEWORK

namespace Persistence

{
    // with identitiyDbContext no necesitamos especificar un nuevo dbset para nuestro AppUser
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }


// esto representa la tabla q crea, y el nombre 
        public DbSet<Activity> Activities {get; set;}


        // si bien llamando a activity podriamos popular y obtener los users.
        // agregamos este db set para poder llamar directamente esta tabla cuando queramos:
        public DbSet<ActivityAttende> ActivityAttendes {get;set;}


        // para poder sobreescribir la configuracion de algun dbset :
        protected override void OnModelCreating(ModelBuilder builder)
        // -> Este método es anulado de la clase base DbContext para proporcionar 
        // configuraciones personalizadas al modelo
        {
            base.OnModelCreating(builder); //  Llama al método base para asegurarse de que cualquier configuración de modelo predeterminada proporcionada por Entity Framework Core se ejecute antes de agregar configuraciones personalizadas

            // Tenemos 2 propertys en nuestra entitie que tienen id, pero queremos 
            // crear una primary key que es una combinacion de app user id y activity id. 
            builder.Entity<ActivityAttende>(x => x.HasKey(aa => new{aa.AppUserId, aa.ActivityId})); // clave principal compuesta

            builder.Entity<ActivityAttende>()
            .HasOne(u => u.AppUser)
            .WithMany(a => a.Activities) // navegacion inversa
            .HasForeignKey(aa => aa.AppUserId);

            // Facilita la navegación entre entidades relacionadas: 
            // Cuando recuperas una instancia de ActivityAttende,
            // tener las propiedades de navegación inversa permite acceder directamente a 
            //la entidad AppUser y Activity relacionada sin tener que escribir manualmente
            //la lógica para recuperar estas entidades por separado.

             builder.Entity<ActivityAttende>()
            .HasOne(u => u.Activity)
            .WithMany(a => a.Attendes) // navegacion inversa
            .HasForeignKey(aa => aa.ActivityId);

        }
    }
}

// DOTNET.EF , UTILITY. ES UN TOOL INSTALADO DESDE EL PACKET MANAGMENT DE .NET "Nuget". para el manejo de ENTITY FRAMEWORK
// COMANDOS PARA MANEJAR DATABASE, EL DBCONTEXT Y LAS MIGRACIONESSSS!!!!
// blesial@MacBook-Air-de-Ignacio Reactivities % dotnet ef migrations add InitialCreate -s API/ -p Persistence