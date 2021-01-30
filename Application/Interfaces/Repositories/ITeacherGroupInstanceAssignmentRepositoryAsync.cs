using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface ITeacherGroupInstanceAssignmentRepositoryAsync : IGenericRepositoryAsync<TeacherGroupInstanceAssignment>
    {
        IEnumerable<TeacherGroupInstanceAssignment> GetByTeacher(string TeacherId);
        TeacherGroupInstanceAssignment GetByGroupInstanceId(int groupInstanceId);
    }
}
