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

namespace Application.DTOs.Account.Commands.DeleteAccountById
{
    public class DeleteUserByIdCommand : IRequest<Response<int>>
    {
        public string Id { get; set; }
        public class DeleteAccountByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, Response<int>>
        {
            private readonly IAccountService _AccountRepository;
            public DeleteAccountByIdCommandHandler(IAccountService AccountRepository)
            {
                _AccountRepository = AccountRepository;
            }
            public async Task<Response<int>> Handle(DeleteUserByIdCommand command, CancellationToken cancellationToken)
            {
                var Account = await _AccountRepository.GetByIdAsync(command.Id);
                if (Account == null) throw new ApiException($"Account Not Found.");
                await _AccountRepository.DeleteAsync(command.Id);
                return new Response<int>(Account.Id);
            }
        }
    }
}
