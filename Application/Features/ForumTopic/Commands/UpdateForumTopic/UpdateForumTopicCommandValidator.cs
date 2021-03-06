using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ForumTopic.Commands.UpdateForumTopic
{
    public class UpdateForumTopicCommandValidator : AbstractValidator<UpdateForumTopicCommand>
    {
        private readonly IForumTopicRepositoryAsync forumtopicRepository;

        public UpdateForumTopicCommandValidator(IForumTopicRepositoryAsync forumtopicRepository)
        {
            this.forumtopicRepository = forumtopicRepository;

        }
    }
}
