using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.OverPaymentStudent.Commands.DeleteOverPaymentStudentById
{
    public class DeleteOverPaymentStudentByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteOverPaymentStudentByIdCommandHandler : IRequestHandler<DeleteOverPaymentStudentByIdCommand, Response<int>>
        {
            private readonly IOverPaymentStudentRepositoryAsync _overpaymentstudentRepository;
            public DeleteOverPaymentStudentByIdCommandHandler(IOverPaymentStudentRepositoryAsync overpaymentstudentRepository)
            {
                _overpaymentstudentRepository = overpaymentstudentRepository;
            }
            public async Task<Response<int>> Handle(DeleteOverPaymentStudentByIdCommand command, CancellationToken cancellationToken)
            {
                var overpaymentstudent = await _overpaymentstudentRepository.GetByIdAsync(command.Id);
                if (overpaymentstudent == null) throw new ApiException($"OverPaymentStudent Not Found.");
                await _overpaymentstudentRepository.DeleteAsync(overpaymentstudent);
                return new Response<int>(overpaymentstudent.Id);
            }
        }
    }
}
