using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateForumCommentCommandValidator : AbstractValidator<CreateForumCommentCommand>
    {
        private readonly IForumCommentRepositoryAsync forumcommentRepository;

        public CreateForumCommentCommandValidator(IForumCommentRepositoryAsync forumcommentRepository)
        {
            this.forumcommentRepository = forumcommentRepository;
        }
    }
}
