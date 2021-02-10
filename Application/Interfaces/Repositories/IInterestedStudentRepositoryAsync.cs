using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IInterestedStudentRepositoryAsync : IGenericRepositoryAsync<InterestedStudent>
    {
        InterestedStudent GetByStudentId(string studentId, int groupDefinitionId);
        List<StudentsModel> GetByGroupDefinitionId(int groupDefinitionId);
        int GetCountOfStudentsByGroupDefinitionId(int groupDefinitionId);
        List<InterestedStudent> GetListByGroupDefinitionId(int groupDefinitionId);
        Task ADDList(List<InterestedStudent> students);
    }
}
