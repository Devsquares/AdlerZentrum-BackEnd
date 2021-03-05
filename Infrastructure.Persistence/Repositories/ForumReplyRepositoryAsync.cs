using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class ForumReplyRepositoryAsync : GenericRepositoryAsync<ForumReply>, IForumReplyRepositoryAsync
    {
        private readonly DbSet<ForumReply> _forumreplys;


        public ForumReplyRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _forumreplys = dbContext.Set<ForumReply>();

        }
    }

}
