using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features 
{
    public class CreateBanRequestCommandValidator : AbstractValidator<CreateBanRequestCommand>
    {
        private readonly IBanRequestRepositoryAsync banrequestRepository;

        public CreateBanRequestCommandValidator(IBanRequestRepositoryAsync banrequestRepository)
        {
            this.banrequestRepository = banrequestRepository;
        }
    }
}
