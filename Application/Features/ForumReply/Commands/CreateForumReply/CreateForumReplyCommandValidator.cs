using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ForumReply.Commands.CreateForumReply
{
    public class CreateForumReplyCommandValidator : AbstractValidator<CreateForumReplyCommand>
    {
        private readonly IForumReplyRepositoryAsync forumreplyRepository;

        public CreateForumReplyCommandValidator(IForumReplyRepositoryAsync forumreplyRepository)
        {
            this.forumreplyRepository = forumreplyRepository;
        }
    }
}
