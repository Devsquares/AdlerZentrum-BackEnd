using Application.Features;
using Application.Helpers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ILessonInstanceRepositoryAsync : IGenericRepositoryAsync<LessonInstance>
    {
        IEnumerable<LessonInstance> GetByGroupInstanceId(int GroupInstanceId);
        Task<Dictionary<int, TimeFrame>> GetTimeSlotInstancesSorted(GroupInstance groupInstance);
        Task<List<LateSubmissionsViewModel>>  GetLateSubmissions(string TeacherName, int pageNumber, int pageSize);
        int GetLateSubmissionsCount(string TeacherName);
    }
}
