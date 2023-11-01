using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Profiles
{
    // La usaremos para el User Profile y para el Attendee Info
    // lo metemos como ICollection dentro del ActivityDto

    // Lo que falta es que indique si el User es Host de la activity -> agregamos esa propiedad en ActivityDto
    public class Profile
    {
        public string UserName {get;set;}

        public string DisplayName {get;set;}

        public string Bio {get;set;}

        public string Image {get;set;}
    }
}