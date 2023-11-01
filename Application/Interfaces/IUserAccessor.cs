namespace Application.Interfaces
{

    // vamos a poder usar esto dentro de nuestro app project. pero la funcionalidad va a estar
    // dentro de nuestro infrastructure project. 
    public interface IUserAccessor
    {
        string GetUser();
    }
}

// Al utilizar la interfaz IUserAccessor, se permite que otras partes del código de 
// la aplicación utilicen métodos definidos en la interfaz sin preocuparse por los 
// detalles de implementación. Esto se conoce como inyección de dependencias.
// Otras partes de la aplicación pueden utilizar IUserAccessor sin saber cómo se implementa
//  realmente, lo que facilita la sustitución de una implementación por otra sin cambiar el 
//código de las partes que la utilizan.