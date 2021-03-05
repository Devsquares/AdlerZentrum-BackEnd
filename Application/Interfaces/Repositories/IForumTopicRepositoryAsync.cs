using Application.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IForumTopicRepositoryAsync : IGenericRepositoryAsync<ForumTopic>
    {
        int GetCount(string userId, ForumType forumType, int groupInstanceId, int groupDefinitionId);
        Task<IReadOnlyList<ForumTopic>> GetPagedReponseAsync(int pageNumber, int pageSize, string userId, ForumType forumType, int groupInstanceId, int groupDefinitionId);
    }
}
