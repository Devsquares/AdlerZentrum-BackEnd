using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateSublevelCommandValidator : AbstractValidator<CreateSublevelCommand>
    {
        private readonly ISublevelRepositoryAsync sublevelRepository;

        public CreateSublevelCommandValidator(ISublevelRepositoryAsync sublevelRepository)
        {
            this.sublevelRepository = sublevelRepository;
        }
    }
}
