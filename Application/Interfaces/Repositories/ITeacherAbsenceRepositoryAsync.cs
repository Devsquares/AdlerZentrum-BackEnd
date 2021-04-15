using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ITeacherAbsenceRepositoryAsync : IGenericRepositoryAsync<TeacherAbsence>
    {
        Task<List<TeacherAbsence>> GetAll(int pageNumber, int pageSize, int? status);
        TeacherAbsence GetbyId(int id);
    }
}
