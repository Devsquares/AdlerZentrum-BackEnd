using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ILessonDefinitionRepositoryAsync : IGenericRepositoryAsync<LessonDefinition>
    {
        Task<ICollection<LessonDefinition>> GetBySubLevelId(int SubLevelId);
    }
}
