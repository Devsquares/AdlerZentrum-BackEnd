using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features 
{
    public class UpdateBanRequestCommandValidator : AbstractValidator<UpdateBanRequestCommand>
    {
        private readonly IBanRequestRepositoryAsync banrequestRepository;

        public UpdateBanRequestCommandValidator(IBanRequestRepositoryAsync banrequestRepository)
        {
            this.banrequestRepository = banrequestRepository;
        }
    }
}
