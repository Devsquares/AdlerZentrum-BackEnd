using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ITestRepositoryAsync : IGenericRepositoryAsync<Test>
    {
        Task<IReadOnlyList<TestsViewModel>> GetPagedReponseAsync(int pageNumber, int pageSize, int? testtype = null, int? levelId = null, int? subLevelId = null, int? testStatus = null);
        Task<Test> GetQuizzByLessonDefinationAsync(int lessonDefinationdId);
        Task<Test> GetSubLevelTestBySublevelAsync(int Sublevel);
        Task<Test> GetFinalLevelTestBySublevelAsync(int level);
        int GetCount(int? testtype = null, int? levelId = null, int? subLevelId = null, int? testStatus = null);
        Task<IReadOnlyList<TestsViewModel>> GetPlacementPagedReponseAsync(int pageNumber, int pageSize, int? testStatus = null);
        int GetPlacementCount();
    }
}
