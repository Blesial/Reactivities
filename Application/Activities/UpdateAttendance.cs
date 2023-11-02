using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class UpdateAttendance
    {
        public class Command : IRequest<Result<Unit>>
        {
            // La prop que necesitamos es el Guid de la activ:
            public Guid Id {get;set;}
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
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
                var activity = await _context.Activities
                    .Include(a => a.Attendes).ThenInclude(u => u.AppUser)
                    // .FirstOrDefaultAsync()
                    // single se asegura que no haya duplicados - a diferencia del first or default.
                    .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (activity == null) return null;

                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUser());

                if (user == null) return null;

                // esto deja de ser asyincrono porque ya tenmos en memoria tanto la activity como los attendees. 
                var HostUserName = activity.Attendes.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

                // ver attendance status para este user en particular:
                var attendance = activity.Attendes.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

                if (attendance != null && HostUserName == user.UserName)
                {
                    activity.IsCancelled = !activity.IsCancelled;
                }

                if (attendance != null && HostUserName != user.UserName)
                {
                    activity.Attendes.Remove(attendance);
                }

                if (attendance == null)
                {
                    attendance = new ActivityAttende
                    {
                        AppUser = user,
                        Activity = activity,
                        IsHost = false
                    };

                    // Sucede solo en memoria

                    activity.Attendes.Add(attendance);
                }

                var result = await _context.SaveChangesAsync() > 0;

                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem Updating Attendance");

            }
        }
    }
}