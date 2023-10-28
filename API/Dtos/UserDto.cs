namespace API.Dtos
{
    // Creamos 1 objeto parausers -> UserDto (tendra propiedades que queremos devolver cuando el usuario se logee o registre).
    public class UserDto
    {
        public string DisplayName {get;set;}

        public string Token {get;set;}

        public string Image {get;set;}

        public string UserName {get;set;}
    }
}