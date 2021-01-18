using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdateSublevelCommandValidator : AbstractValidator<UpdateSublevelCommand>
    {
        private readonly ISublevelRepositoryAsync sublevelRepository;

        public UpdateSublevelCommandValidator(ISublevelRepositoryAsync sublevelRepository)
        {
            this.sublevelRepository = sublevelRepository;

        }
    }
}
