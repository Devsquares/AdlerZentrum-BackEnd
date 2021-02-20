﻿using Domain.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IGroupInstanceStudentRepositoryAsync : IGenericRepositoryAsync<GroupInstanceStudents>
    {
        int GetCountOfStudents(int groupId);
        List<GroupInstanceModel> GetAllLastByStudentId(string studentId);
    }
}
