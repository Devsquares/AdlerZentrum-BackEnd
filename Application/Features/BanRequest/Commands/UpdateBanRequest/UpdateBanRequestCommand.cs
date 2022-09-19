using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Features
{
    public class UpdateBanRequestCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int status { get; set; }
        public string Comment { get; set; }

        public class UpdateBanRequestCommandHandler : IRequestHandler<UpdateBanRequestCommand, Response<int>>
        {
            private readonly IAccountService _accountService;
            private readonly IBanRequestRepositoryAsync _banrequestRepository;
            private readonly IMailJobRepositoryAsync _jobRepository;
            public UpdateBanRequestCommandHandler(IBanRequestRepositoryAsync banrequestRepository,
                IAccountService accountService,
                 IMailJobRepositoryAsync mailJobRepositoryAsync)
            {
                _banrequestRepository = banrequestRepository;
                _accountService = accountService;
                _jobRepository = mailJobRepositoryAsync;
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
                    banrequest.BanRequestStatus = command.status;
                    banrequest.Comment = command.Comment;
                    if (banrequest.BanRequestStatus == (int)BanRequestStatusEnum.Approved)
                    {
                        await _accountService.BanAsync(banrequest.StudentId, command.Comment);
                        await _jobRepository.AddAsync(new MailJob
                        {
                            Type = (int)MailJobTypeEnum.Banning,
                            StudentId = banrequest.StudentId,
                            Status = (int)JobStatusEnum.New
                        });
                    }
                    await _banrequestRepository.UpdateAsync(banrequest);
                    return new Response<int>(banrequest.Id);
                }
            }
        }

    }
}
