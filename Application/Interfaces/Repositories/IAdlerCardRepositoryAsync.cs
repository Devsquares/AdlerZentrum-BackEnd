using Application.Features;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IAdlerCardRepositoryAsync : IGenericRepositoryAsync<AdlerCard>
    {
        List<GetAdlerCardGroupsForStudentViewModel> GetAdlerCardGroupsForStudent();
        List<AdlerCard> GetAllByUnitId(int unitId);
    }
}
