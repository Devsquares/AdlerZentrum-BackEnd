using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface ILessonInstanceRepositoryAsync : IGenericRepositoryAsync<LessonInstance>
    {
        IEnumerable<LessonInstance> GetByGroupInstanceId(int GroupInstanceId);
    }
}
