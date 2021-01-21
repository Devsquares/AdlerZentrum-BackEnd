using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateListeningAudioFileCommandValidator : AbstractValidator<CreateListeningAudioFileCommand>
    {
        private readonly IListeningAudioFileRepositoryAsync listeningaudiofileRepository;

        public CreateListeningAudioFileCommandValidator(IListeningAudioFileRepositoryAsync listeningaudiofileRepository)
        {
            this.listeningaudiofileRepository = listeningaudiofileRepository;
        }
    }
}
