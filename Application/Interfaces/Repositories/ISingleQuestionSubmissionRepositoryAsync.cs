using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ISingleQuestionSubmissionRepositoryAsync : IGenericRepositoryAsync<SingleQuestionSubmission>
    {
        Task<IReadOnlyList<SingleQuestionSubmission>> GetByTestInstanceIdAsync(int TestInstanceId);
    }
}
