using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IAdlerCardSubmissionRepositoryAsync : IGenericRepositoryAsync<AdlerCardSubmission>
    {
        AdlerCardSubmission GetAdlerCardForStudent(string studentId, int adlercardId);
        IEnumerable<AdlerCardsSubmissionsForStaffModel> GetAdlerCardsSubmissionsForStaff(int pageNumber, int pageSize, string studentId, string studentName, int? levelId, string levelName, int? type, int? status, bool? assigned, string TeacherId, out int totalCount);
    }
}
