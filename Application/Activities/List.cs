using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {

        // solicitud:
        public class Query : IRequest<Result<List<ActivityDto>>> { }

        // handler de esa solicitud:
        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext _context;
        private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
            _mapper = mapper;
                _context = context;

            }

            // Response: 
            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // try
                // {
                //     for (var i = 0; i < 10; i++)
                //     {
                //         cancellationToken.ThrowIfCancellationRequested();
                //         await Task.Delay(1000, cancellationToken);
                //         _logger.LogInformation($"Task {i} has completed");
                //     }
                // }
                // catch (System.Exception)
                // {

                //     _logger.LogInformation("Task was cancelled");
                // }
                
                // habria que tambien desde nuestro controller enviarnos en el query el cancellationToken, ya que la api es la que recibe el request y luego recien nos manda la query aqui. 
                var activities = await _context.Activities
                .Include(a => a.Attendes) // este es solo nuestro join table. pero debemos tmb incluir los usuarios relacionados con este join table:
                .ThenInclude(b => b.AppUser)
                .ToListAsync(cancellationToken);

                // PUEDE QUE OBTENGAMOS EL ERROR DE "A POSSIBLE OBJECT CYCLE WAS DETECTED"
                // PARA SOLUCIONARLO -> ESTO SUCEDE PORQUE AL INCLUIR LOS APPUSERS, ESTA ENTITIE CONTIENE TAMBIEN UNA LISTA DE ACTIVITY ATTENDEES. Y DENTRO HAY ACTIVITIES Y AP USERS, ETC. 
                // hay que modificar nuestro data "SHAPING OUR DATA"
                 
                 // indicamos que mapearemos a una lista de activity DTO : 
                var activitiesToReturn = _mapper.Map<List<ActivityDto>>(activities);
                 return Result<List<ActivityDto>>.Success(activitiesToReturn);          
            }   
        }
    }
}