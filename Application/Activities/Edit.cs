
using Application.Activities;
using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity {get;set;}
        }

         public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
            _mapper = mapper;
            _context = context;
            }

            // CANCELLATION TOKEN: SI LA LOGICA DENTRO DE LA CLASE HANDLE ES MUY COMPLEJA Y TARDARIA TIEMPO.
            // SI EL REQUEST TARDA DIGAMOS 30 SEG , Y POR EJ VOS COMO CLIENTE BORRAS POSTMAN . NADA PARECERIA QUE ESTA PASANDO.
            // PERO ESE REQUEST SERIA CORRIENDO, Y NOS DEVOLVERIA TARDE O TEMPRANO ALGO, PERO LO MALO ES QUE EL SERVER SIGUE LABURANDO
            // ESTO NOS PERMITE CANCELAR UN REQUEST SI YA NO ES NECESARIO O NO LO QUEREMOS MAS. 
            public async Task <Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // here we are tracking the activity in memory
               var activity = await _context.Activities.FindAsync(request.Activity.Id);

                // esto es sin automapper: (para cada propiedad habria q hacerlo manualmente)
                // activity.Title = request.Activity.Title ?? activity.Title;

                if (activity == null) return null;

                //luego de inyectar imapper, y ejecutamos el map donde decimos desde donde hacia donde. 
                // automapper toma todas las props del primero y las updatea en el segundo 
                // RECORDAR AGREGAR EL AUTOMAPPER AS A SERVICE EN PROGRAM 
                _mapper.Map(request.Activity, activity);

               // nuestra bd gets updated
                var result = await _context.SaveChangesAsync() > 0;

               if (!result) return Result<Unit>.Failure("Failed to update activity");

               return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}