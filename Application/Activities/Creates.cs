using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Creates
    {
        // unit viene del mediator y es un void . usamos esto porque el create devuelve un void. 
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        { // estamos validando contra la clase command -> lo q nos da acceso a establecer un rule para Activities.
            public CommandValidator()
            {
                // instanciando la clase activity validator es q estamos ejecutando todos los rules de su constructor contra la activity recibida. 
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // adding activity IN MEMORY, no in the data base yet. EF is TRACKING 
                _context.Activities.Add(request.Activity);

               var result = await _context.SaveChangesAsync() > 0;

               if (!result) Result<Unit>.Failure("Failed to create Activity");

               return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}