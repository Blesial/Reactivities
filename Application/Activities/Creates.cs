using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Creates // está dividida en tres clases internas:
    {
        // unit viene del mediator y es un void . usamos esto porque el create devuelve un void. 
        //Define un comando para crear una actividad. Este comando se utilizará con MediatR para enviar una solicitud y manejarla.
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command> // Realiza la validación de la solicitud de creación de actividad.
        { // estamos validando contra la clase command -> lo q nos da acceso a establecer un rule para Activities.
            public CommandValidator()
            {
                // instanciando la clase activity validator es q estamos ejecutando todos los rules de su constructor contra la activity recibida. 
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        // Maneja la solicitud de creación de actividad, ejecutando la lógica de creación y persistencia en la base de datos.
        {
            private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // GET USER:
                var user = await _context.Users.FirstOrDefaultAsync(x => 
                    x.UserName == _userAccessor.GetUser());
                
                // con el user podremos crear el nuevo attendee:
                var attendee = new ActivityAttende
                {
                    AppUser = user,
                    Activity = request.Activity,
                    IsHost = true
                };

                request.Activity.Attendes.Add(attendee);


                // adding activity IN MEMORY, no in the data base yet. EF is TRACKING 
                _context.Activities.Add(request.Activity);

               var result = await _context.SaveChangesAsync() > 0;

               if (!result) Result<Unit>.Failure("Failed to create Activity");

               return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}


// Esta estructura permite una separación clara de responsabilidades:
// el comando define qué se hace, el validador asegura que sea válido y el 
//manejador ejecuta la acción y maneja la persistencia. Esto facilita el mantenimiento 
// y la escalabilidad del código al separar distintas responsabilidades en cada clase.