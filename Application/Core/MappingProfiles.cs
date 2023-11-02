using Application.Activities;
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
            CreateMap<Activity, ActivityDto>()
            // Configuracion para el automapper para related data :
            // 
            .ForMember(d => d.HostUserName, o => o.MapFrom(s => s.Attendes
            .FirstOrDefault(x => x.IsHost).AppUser.UserName));

            // necesitamos agregar otro mapping debido al mapping error porque dentro del activityDto tenemos un Attende property y no es llamado profile.
            // 
            CreateMap<ActivityAttende, Profiles.Profile>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName)) 
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.AppUser.UserName)) 
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio)); 
        }

        // o => o.MapFrom(s => s.Attendes.FirstOrDefault(x => x.IsHost).AppUser.UserName): Specifies how to derive the value for HostUserName in the ActivityDto.
// s represents the source object (Activity instance).
// s.Attendes refers to a collection (presumably a list) of related entities in the Activity class.
// FirstOrDefault(x => x.IsHost) finds the first related entity in Attendes where IsHost property is true.
// .AppUser.UserName accesses the UserName property of the AppUser entity related to the IsHost attendee.
    }
}