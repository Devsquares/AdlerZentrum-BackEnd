using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AdlerCardBundleStudent.Commands.DeleteAdlerCardBundleStudentById
{
    public class DeleteAdlerCardBundleStudentByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteAdlerCardBundleStudentByIdCommandHandler : IRequestHandler<DeleteAdlerCardBundleStudentByIdCommand, Response<int>>
        {
            private readonly IAdlerCardBundleStudentRepositoryAsync _adlercardbundlestudentRepository;
            private readonly IAccountService _accountService;

            public DeleteAdlerCardBundleStudentByIdCommandHandler(IAdlerCardBundleStudentRepositoryAsync adlercardbundlestudentRepository, IAccountService accountService)
            {
                _adlercardbundlestudentRepository = adlercardbundlestudentRepository;
                _accountService = accountService;
            }
            public async Task<Response<int>> Handle(DeleteAdlerCardBundleStudentByIdCommand command, CancellationToken cancellationToken)
            {
                var adlercardbundlestudent =  _adlercardbundlestudentRepository.GetByBundleID(command.Id);
                if (adlercardbundlestudent == null) throw new ApiException($"AdlerCardBundleStudent Not Found.");
                await _accountService.UpdateAdlerCardBalance(adlercardbundlestudent.StudentId,(-adlercardbundlestudent.AdlerCardsBundle.Count));
                adlercardbundlestudent.Student = null;
                adlercardbundlestudent.AdlerCardsBundle = null;
                await _adlercardbundlestudentRepository.DeleteAsync(adlercardbundlestudent);
                return new Response<int>(adlercardbundlestudent.Id);
            }
        }
    }
}
