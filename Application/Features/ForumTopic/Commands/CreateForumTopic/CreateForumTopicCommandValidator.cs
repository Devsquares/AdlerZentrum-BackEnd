using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ForumTopic.Commands.CreateForumTopic
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
