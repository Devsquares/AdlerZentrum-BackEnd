using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdateForumCommentCommandValidator : AbstractValidator<UpdateForumCommentCommand>
    {
        private readonly IForumCommentRepositoryAsync forumcommentRepository;

        public UpdateForumCommentCommandValidator(IForumCommentRepositoryAsync forumcommentRepository)
        {
            this.forumcommentRepository = forumcommentRepository;

        }
    }
}
