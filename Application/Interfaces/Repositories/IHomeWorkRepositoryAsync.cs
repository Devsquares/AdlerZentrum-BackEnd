using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface IHomeWorkRepositoryAsync : IGenericRepositoryAsync<Homework>
    {
        ICollection<Homework> GetAllBounsRequests();
        Homework GetByLessonInstance(int LessonInstanceId);
    }
}
