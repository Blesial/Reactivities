using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
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
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }


// esto representa la tabla q crea, y el nombre 
        public DbSet<Activity> Activities {get; set;}
    }
}

// DOTNET.EF , UTILITY. ES UN TOOL INSTALADO DESDE EL PACKET MANAGMENT DE .NET "Nuget". para el manejo de ENTITY FRAMEWORK
// COMANDOS PARA MANEJAR DATABASE, EL DBCONTEXT Y LAS MIGRACIONESSSS!!!!
// blesial@MacBook-Air-de-Ignacio Reactivities % dotnet ef migrations add InitialCreate -s API/ -p Persistence