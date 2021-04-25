using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface ITeacherGroupInstanceAssignmentRepositoryAsync : IGenericRepositoryAsync<TeacherGroupInstanceAssignment>
    {
        IEnumerable<TeacherGroupInstanceAssignment> GetByTeacher(string TeacherId);
        TeacherGroupInstanceAssignment GetByGroupInstanceId(int groupInstanceId);
        TeacherGroupInstanceAssignment GetByTeachGroupInstanceId(string TeacherId, int groupInstanceId);
        List<TeacherGroupInstanceAssignment> GetListByGroupInstanceId(int groupInstanceId);
        object GetAll(int pageNumber, int pageSize, out int totalCount, int? sublevelId = null, int? groupDefinationId = null);
        TeacherGroupInstanceAssignment GetFirstNotIsDefault(int groupInstanceId);
        TeacherGroupInstanceAssignment GetByTeacherId(string TeacherId);
        public TeacherGroupInstanceAssignment GetByGroupInstanceIdWithoutDefault(int groupInstanceId);
    }
}
