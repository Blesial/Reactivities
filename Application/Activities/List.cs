using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {

        // solicitud:
        public class Query : IRequest<List<Activity>> { }

        // handler de esa solicitud:
        public class Handler : IRequestHandler<Query, List<Activity>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;

            }

            // Response: 
            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
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
                return await _context.Activities.ToListAsync();
            
            }
        }
    }
}