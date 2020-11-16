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

namespace Application.DTOs.Account.Commands.UpdateAccount
{
    public class UpdateBusinessUserCommand : IRequest<Response<int>>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Language { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string TelefonNumber { get; set; }

        public string Title { get; set; }

        public string ContactEmail { get; set; }
        public string ContactTelefonNumber { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ContactFaxNumber { get; set; }
        public string ContactLanguge { get; set; }

        public string CompanyName { get; set; }
        public string CommercialName { get; set; }

        public class UpdateAccountCommandHandler : IRequestHandler<UpdateBusinessUserCommand, Response<int>>
        {
            private readonly IAccountService _AccountRepository;
            public UpdateAccountCommandHandler(IAccountService AccountRepository)
            {
                _AccountRepository = AccountRepository;
            }
            public async Task<Response<int>> Handle(UpdateBusinessUserCommand command, CancellationToken cancellationToken)
            {
                var Account = await _AccountRepository.GetByIdAsync(command.Id);

                if (Account == null)
                {
                    throw new ApiException($"Account Not Found.");
                }
                else
                {
                    await _AccountRepository.UpdateAsync(command);
                    return new Response<int>(Account.Id);
                }
            }
        }
    }
}
