using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class ForumCommentRepositoryAsync : GenericRepositoryAsync<ForumComment>, IForumCommentRepositoryAsync
    {
        private readonly DbSet<ForumComment> _forumComments;
        private readonly ApplicationDbContext _dbContext;


        public ForumCommentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _forumComments = _dbContext.Set<ForumComment>();

        }

        public async Task<IReadOnlyList<ForumComment>> GetPagedReponseAsync(int pageNumber, int pageSize, int forumTopicId)
        {
            return await _dbContext.Set<ForumComment>()
                .Include(b=>b.Writer)
                .Include(b=>b.ForumReplys)
                .Where(b => (forumTopicId == 0 || b.ForumTopicId == forumTopicId))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public int GetCount(int forumTopicId)
        {
            return _dbContext.Set<ForumComment>()
                .Where(b => (forumTopicId == 0 || b.ForumTopicId == forumTopicId))
                .Count();
        }
    }

}
