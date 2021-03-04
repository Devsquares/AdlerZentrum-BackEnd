using Application.Features;
using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IAdlerCardRepositoryAsync : IGenericRepositoryAsync<AdlerCard>
    {
        List<GetAdlerCardGroupsForStudentViewModel> GetAdlerCardGroupsForStudent();
        List<AdlerCard> GetAllByUnitId(int unitId);
        List<AdlerCardModel> GetAdlerCardsForStudent(string studentId, int adlerCardUnitId);
    }
}
