namespace Application.Core
{
    public class Result<T> // generic type
    {
        public bool IsSuccess {get;set;}

        public T Value {get;set;}

        public string Error {get;set;}


    // metodos para llamar dentro de los handlers y pasarles la entidad -> en este caso seria pasarle la activity
    // puede ser la activity que se encontro, o puede ser null 
        public static Result<T> Success(T value) => new() { IsSuccess=true, Value=value};
        // esto es para menjar un error q haya pasado dentro del handler  
        public static Result<T> Failure(string error) => new() { IsSuccess=false, Error=error};


    }
}