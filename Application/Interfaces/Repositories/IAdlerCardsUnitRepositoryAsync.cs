using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IAdlerCardsUnitRepositoryAsync : IGenericRepositoryAsync<AdlerCardsUnit>
    {
        List<GetAdlerCardUnitsForStudentViewModel> GetAdlerCardUnitsForStudent(string StudentId,int LevelId,int AdlerCardTypeId);
        List<AdlerCardsUnit> GetAdlerCardUnitsByLevelAndType(int levelId, int adlerCardTypeId);
        public AdlerCardsUnit GetAdlerCardsUnitById(int id);
    }
}
