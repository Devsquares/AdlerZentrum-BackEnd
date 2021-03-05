using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IForumCommentRepositoryAsync : IGenericRepositoryAsync<ForumComment>
    {
        int GetCount(int forumTopicId);
        Task<IReadOnlyList<ForumComment>> GetPagedReponseAsync(int pageNumber, int pageSize, int forumTopicId);
    }
}
