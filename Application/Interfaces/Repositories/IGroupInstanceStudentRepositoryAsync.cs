﻿using Domain.Entities;
using Domain.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IGroupInstanceStudentRepositoryAsync : IGenericRepositoryAsync<GroupInstanceStudents>
    {
        int GetCountOfStudents(int groupId);

        List<string> GetEmailsByGroupDefinationId(int groupDefinationId);
        GroupInstanceStudents GetByStudentId(string studentId, int groupId);
        Task<int> GetCountOfPlacmentTestStudents(int groupId);
        Task<int> GetCountOfStudentsByGroupDefinitionId(int groupDefinitionId);
        GroupInstance GetLastByStudentId(string studentId);
        List<GroupInstance> GetAllLastByStudentId(string studentId);
        Task<List<ApplicationUser>> GetAllStudentInGroupInstanceByStudentId(string studentId);
        Task<List<ApplicationUser>> GetAllStudentInGroupDefinitionByStudentId(string studentId);

        void SaveAllGroupInstanceStudents(int groupDefinitionId, List<StudentsGroupInstanceModel> groupInstanceStudentslist);
        List<IGrouping<int, GroupInstanceStudents>> GetAllByGroupDefinition(int? groupDefinitionId= null, int? groupInstanceId = null);
        List<GroupInstanceStudents> GetByGroupDefinitionAndGroupInstance(int groupDefinitionId, int? groupinstanceId = null);
    }
}
