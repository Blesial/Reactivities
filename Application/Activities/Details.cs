using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<ActivityDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
        {
        private readonly IMapper _mapper;

            private readonly DataContext _context;
            public Handler(DataContext context, IMapper mapper)
            {
            _mapper = mapper;
                _context = context;

            }
            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities
                // NO SE PUEDE FINDASYNC CON PROYECTION. CAMBIAREMOS A FIRST OR DEFAULT
                // La razón por la que no se puede usar FindAsync directamente con proyecciones 
                // se debe a que FindAsync está diseñado para buscar una entidad completa por su clave primaria. 
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

                // no vamos aespecificar failure porque no lo necesitamos, nuestro activity va a ser el activity o va a ser null
                // dsp dentro del controller vamos a testear que tipo response obtenemos. 

                return Result<ActivityDto>.Success(activity);
            }
        }
    }
}