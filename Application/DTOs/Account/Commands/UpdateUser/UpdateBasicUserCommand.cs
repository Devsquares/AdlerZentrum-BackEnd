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
    public class UpdateBasicUserCommand : IRequest<Response<Domain.Entities.Account>>
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public class UpdateAccountCommandHandler : IRequestHandler<UpdateBasicUserCommand, Response<Domain.Entities.Account>>
        {
            private readonly IAccountService _AccountRepository;
            public UpdateAccountCommandHandler(IAccountService AccountRepository)
            {
                _AccountRepository = AccountRepository;
            }
            public async Task<Response<Domain.Entities.Account>> Handle(UpdateBasicUserCommand command, CancellationToken cancellationToken)
            {
                var Account = await _AccountRepository.GetByIdAsync(command.Id);

                if (Account == null)
                {
                    throw new ApiException($"Account Not Found.");
                }
                else
                {
                    var res = await _AccountRepository.UpdateAsync(command);
                    if (res.Succeeded)
                    {
                        Reflection.CopyProperties(command,Account);
                        return new Response<Domain.Entities.Account>(Account, res.Succeeded.ToString());
                    }
                    else
                    {

                        return new Response<Domain.Entities.Account>(Account);
                    }
                }
            }
        }
    }
}


