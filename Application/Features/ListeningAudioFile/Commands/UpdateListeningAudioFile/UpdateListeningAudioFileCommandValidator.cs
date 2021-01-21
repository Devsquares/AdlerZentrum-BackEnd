using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdateListeningAudioFileCommandValidator : AbstractValidator<UpdateListeningAudioFileCommand>
    {
        private readonly IListeningAudioFileRepositoryAsync listeningaudiofileRepository;

        public UpdateListeningAudioFileCommandValidator(IListeningAudioFileRepositoryAsync listeningaudiofileRepository)
        {
            this.listeningaudiofileRepository = listeningaudiofileRepository;

        }
    }
}
