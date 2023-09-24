using AutoMapper;
using Domain;


// Features que tengan q ver con nuestra carpeta aplication folder. no especificamente para activities. 


namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // mapping from an activity to an activity
            // automapper va a mirar las propiedades dentro de esta clase (id, title, description, etc)
            // matchea las propiedades y las actualiza por nosotros. 
            CreateMap<Activity, Activity>();
        }
    }
}