using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IAdlerCardSubmissionRepositoryAsync : IGenericRepositoryAsync<AdlerCardSubmission>
    {
        AdlerCardSubmission GetAdlerCardForStudent(string studentId, int adlercardId);
    }
}
