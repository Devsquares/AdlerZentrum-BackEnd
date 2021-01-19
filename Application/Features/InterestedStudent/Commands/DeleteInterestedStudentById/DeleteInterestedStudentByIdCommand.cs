using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InterestedStudent.Commands.DeleteInterestedStudentById
{
    public class DeleteInterestedStudentByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteInterestedStudentByIdCommandHandler : IRequestHandler<DeleteInterestedStudentByIdCommand, Response<int>>
        {
            private readonly IInterestedStudentRepositoryAsync _interestedstudentRepository;
            public DeleteInterestedStudentByIdCommandHandler(IInterestedStudentRepositoryAsync interestedstudentRepository)
            {
                _interestedstudentRepository = interestedstudentRepository;
            }
            public async Task<Response<int>> Handle(DeleteInterestedStudentByIdCommand command, CancellationToken cancellationToken)
            {
                var interestedstudent = await _interestedstudentRepository.GetByIdAsync(command.Id);
                if (interestedstudent == null) throw new ApiException($"InterestedStudent Not Found.");
                await _interestedstudentRepository.DeleteAsync(interestedstudent);
                return new Response<int>(interestedstudent.Id);
            }
        }
    }
}
