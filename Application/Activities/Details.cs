using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<Activity>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Activity>>
        {

            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;

            }
            public async Task<Result<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);

                // no vamos aespecificar failure porque no lo necesitamos, nuestro activity va a ser el activity o va a ser null
                // dsp dentro del controller vamos a testear que tipo response obtenemos. 

                return Result<Activity>.Success(activity);
            }
        }
    }
}