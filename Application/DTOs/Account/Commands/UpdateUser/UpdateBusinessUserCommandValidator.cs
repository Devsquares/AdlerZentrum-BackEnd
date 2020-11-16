using Application.Interfaces;
using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account.Commands.UpdateAccount
{
    public class UpdateBusinessAccountCommandValidator : AbstractValidator<UpdateBusinessUserCommand>
    {
        private readonly IAccountService AccountRepository;

        public UpdateBusinessAccountCommandValidator(IAccountService AccountRepository)
        {
            this.AccountRepository = AccountRepository;

        }
    }
}
