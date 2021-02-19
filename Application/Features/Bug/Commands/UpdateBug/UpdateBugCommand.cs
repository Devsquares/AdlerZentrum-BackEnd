using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Bug.Commands.UpdateBug
{
	public class UpdateBugCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public string UserName { get; set; }
		public string BugName { get; set; }
		public string Type { get; set; }
		public string Priority { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
        public string Status { get; set; }

        public class UpdateBugCommandHandler : IRequestHandler<UpdateBugCommand, Response<int>>
        {
            private readonly IBugRepositoryAsync _bugRepository;
            public UpdateBugCommandHandler(IBugRepositoryAsync bugRepository)
            {
                _bugRepository = bugRepository;
            }
            public async Task<Response<int>> Handle(UpdateBugCommand command, CancellationToken cancellationToken)
            {
                var bug = await _bugRepository.GetByIdAsync(command.Id);

                if (bug == null)
                {
                    throw new ApiException($"Bug Not Found.");
                }
                else
                {
				    bug.UserName = command.UserName;
				    bug.BugName = command.BugName;
				    bug.Type = command.Type;
				    bug.Priority = command.Priority;
				    bug.Description = command.Description;
				    bug.Image = command.Image;
                    bug.Status = command.Status;

                    await _bugRepository.UpdateAsync(bug);
                    return new Response<int>(bug.Id);
                }
            }
        }

    }
}
