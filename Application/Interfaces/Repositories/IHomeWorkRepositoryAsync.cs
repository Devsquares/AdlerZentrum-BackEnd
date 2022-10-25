using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface IHomeWorkRepositoryAsync : IGenericRepositoryAsync<Homework>
    {
        ICollection<Homework> GetAllBounsRequests(int pageNumber, int pageSize, int? status);

        int GetAllBounsRequestsCount(int? status);
        Homework GetByLessonInstance(int LessonInstanceId);
    }
}
