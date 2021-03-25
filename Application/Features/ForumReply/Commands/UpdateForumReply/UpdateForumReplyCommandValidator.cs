using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdateForumReplyCommandValidator : AbstractValidator<UpdateForumReplyCommand>
    {
        private readonly IForumReplyRepositoryAsync forumreplyRepository;

        public UpdateForumReplyCommandValidator(IForumReplyRepositoryAsync forumreplyRepository)
        {
            this.forumreplyRepository = forumreplyRepository;

        }
    }
}
