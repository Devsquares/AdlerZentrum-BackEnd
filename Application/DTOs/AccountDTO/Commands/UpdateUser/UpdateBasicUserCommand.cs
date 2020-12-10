using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.Account.Commands.UpdateAccount
{
    public class UpdateBasicUserCommand : IRequest<Response<AccountViewModel>>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual bool Active { get; set; }
        public virtual bool Deleted { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Profilephoto { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public class UpdateAccountCommandHandler : IRequestHandler<UpdateBasicUserCommand, Response<AccountViewModel>>
        {
            private readonly IAccountService _AccountRepository;
            private readonly IMapper _mapper;
            public UpdateAccountCommandHandler(IAccountService AccountRepository, IMapper mapper)
            {
                _AccountRepository = AccountRepository;
                _mapper = mapper;
            }
            public async Task<Response<AccountViewModel>> Handle(UpdateBasicUserCommand command, CancellationToken cancellationToken)
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
                        Reflection.CopyProperties(command, Account);

                        var userViewModel = _mapper.Map<AccountViewModel>(Account);
                        return new Response<AccountViewModel>(userViewModel, res.Succeeded.ToString());
                    }
                    else
                    {
                        return new Response<AccountViewModel>("Faild to update");
                    }
                }
            }
        }
    }
}


