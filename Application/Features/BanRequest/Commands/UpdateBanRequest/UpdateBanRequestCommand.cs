using Application.Enums;
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

namespace Application.Features 
{
    public class UpdateBanRequestCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int BanRequestStatus { get; set; }
        public string Comment { get; set; }

        public class UpdateBanRequestCommandHandler : IRequestHandler<UpdateBanRequestCommand, Response<int>>
        {
            private readonly IAccountService _accountService;
            private readonly IBanRequestRepositoryAsync _banrequestRepository;
            public UpdateBanRequestCommandHandler(IBanRequestRepositoryAsync banrequestRepository,
                IAccountService accountService)
            {
                _banrequestRepository = banrequestRepository;
                _accountService = accountService;
            }
            public async Task<Response<int>> Handle(UpdateBanRequestCommand command, CancellationToken cancellationToken)
            {
                var banrequest = await _banrequestRepository.GetByIdAsync(command.Id);

                if (banrequest == null)
                {
                    throw new ApiException($"BanRequest Not Found.");
                }
                else
                {
                    banrequest.BanRequestStatus = command.BanRequestStatus;
                    banrequest.Comment = command.Comment;
                    if (banrequest.BanRequestStatus == (int)BanRequestStatusEnum.Approved)
                    {
                        await _accountService.BanAsync(banrequest.StudentId, command.Comment);
                    }
                    await _banrequestRepository.UpdateAsync(banrequest);
                    return new Response<int>(banrequest.Id);
                }
            }
        }

    }
}
