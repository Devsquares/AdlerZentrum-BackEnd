using Application.Interfaces;
using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account.Commands.UpdateAccount
{
    public class UpdateBasicAccountCommandValidator : AbstractValidator<UpdateBasicUserCommand>
    {
        private readonly IAccountService AccountRepository;

        public UpdateBasicAccountCommandValidator(IAccountService AccountRepository)
        {
            this.AccountRepository = AccountRepository;

        }
    }
}
