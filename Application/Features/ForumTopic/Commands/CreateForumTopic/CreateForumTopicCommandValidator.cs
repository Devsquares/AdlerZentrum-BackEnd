using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateForumTopicCommandValidator : AbstractValidator<CreateForumTopicCommand>
    {
        private readonly IForumTopicRepositoryAsync forumtopicRepository;

        public CreateForumTopicCommandValidator(IForumTopicRepositoryAsync forumtopicRepository)
        {
            this.forumtopicRepository = forumtopicRepository;
        }
    }
}
