using Application.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class GroupInstanceStudentRepositoryAsync : GenericRepositoryAsync<GroupInstanceStudents>, IGroupInstanceStudentRepositoryAsync
    {
        private readonly DbSet<GroupInstanceStudents> groupInstanceStudents;
        private readonly DbSet<GroupInstance> groupInstances;
        private readonly DbSet<GroupDefinition> groupDefinition;
        public GroupInstanceStudentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            groupInstanceStudents = dbContext.Set<GroupInstanceStudents>();
            groupInstances = dbContext.Set<GroupInstance>();
            groupDefinition = dbContext.Set<GroupDefinition>();
        }

        public GroupInstanceStudents GetByStudentId(string studentId, int groupId)
        {
            return groupInstanceStudents.Where(x => x.StudentId == studentId && x.GroupInstanceId == groupId).FirstOrDefault();
        }

        public int GetCountOfStudents(int groupId)
        {
            return groupInstanceStudents.Where(x => x.GroupInstanceId == groupId).Count();
        }

        public List<string> GetEmailsByGroupDefinationId(int groupDefinationId)
        {
            var emailList = groupInstanceStudents.Include(x => x.GroupInstance)
                 .Include(x => x.Student)
                 .Where(x => x.GroupInstance.GroupDefinitionId == groupDefinationId).Select(x => x.Student.Email).ToList();
            return emailList;
        }

        public async Task<int> GetCountOfPlacmentTestStudents(int groupId)
        {
            return await groupInstanceStudents.Where(x => x.GroupInstanceId == groupId && x.IsPlacementTest == true).CountAsync();
        }

        public async Task<int> GetCountOfStudentsByGroupDefinitionId(int groupDefinitionId)
        {
            return await groupInstanceStudents.Include(x => x.GroupInstance).Where(x => x.GroupInstance.GroupDefinitionId == groupDefinitionId).CountAsync();
        }

        public GroupInstanceModel GetLastByStudentId(string studentId)
        {
            return groupInstanceStudents.Include(x => x.GroupInstance.GroupDefinition.TimeSlot)
                .Where(x => x.StudentId == studentId && x.IsDefault == true)
                .Select(x => new GroupInstanceModel()
                {
                    GroupDefinitionId = x.GroupInstance.GroupDefinitionId,
                    GroupDefinitionStartDate = x.GroupInstance.GroupDefinition.StartDate,
                    GroupDefinitionEndDate = x.GroupInstance.GroupDefinition.EndDate,
                    GroupDefinitionFinalTestDate = x.GroupInstance.GroupDefinition.FinalTestDate,
                    Serial = x.GroupInstance.Serial,
                    Status = x.GroupInstance.Status,
                    CreatedDate = x.GroupInstance.CreatedDate,
                    IsCurrent = x.IsDefault,
                    GroupInstanceId = x.GroupInstance.Id,
                    TimeSlots = x.GroupInstance.GroupDefinition.TimeSlot

                }).FirstOrDefault();
        }
        public List<GroupInstanceModel> GetAllLastByStudentId(string studentId)
        {
            return groupInstanceStudents.Include(x => x.GroupInstance.GroupDefinition.TimeSlot)
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new GroupInstanceModel() { 
                    GroupDefinitionId = x.GroupInstance.GroupDefinitionId,
                    GroupDefinitionStartDate = x.GroupInstance.GroupDefinition.StartDate,
                    GroupDefinitionEndDate = x.GroupInstance.GroupDefinition.EndDate,
                    GroupDefinitionFinalTestDate = x.GroupInstance.GroupDefinition.FinalTestDate,
                    Serial = x.GroupInstance.Serial,
                    Status = x.GroupInstance.Status,
                    CreatedDate = x.GroupInstance.CreatedDate,
                    IsCurrent = x.IsDefault,
                    GroupInstanceId = x.GroupInstance.Id,
                    TimeSlots = x.GroupInstance.GroupDefinition.TimeSlot

                })
                .ToList();
        }

        public async Task<List<ApplicationUser>> GetAllStudentInGroupInstanceByStudentId(string studentId)
        {
            var groupinstance = await groupInstanceStudents.Where(x => x.StudentId == studentId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
            return await groupInstanceStudents.Include(x => x.Student).Where(x => x.GroupInstanceId == groupinstance.GroupInstanceId).Select(x => x.Student).ToListAsync();

        }
        public async Task<List<ApplicationUser>> GetAllStudentInGroupDefinitionByStudentId(string studentId)
        {
            var groupinstance = await groupInstanceStudents.Include(x => x.GroupInstance).Where(x => x.StudentId == studentId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
            return await groupInstanceStudents.Include(x => x.Student).Where(x => x.GroupInstance.GroupDefinitionId == groupinstance.GroupInstance.GroupDefinitionId).Select(x => x.Student).ToListAsync();
        }

        public List<GroupInstanceStudents> SaveAllGroupInstanceStudents(int groupDefinitionId, List<StudentsGroupInstanceModel> groupInstanceStudentslist,out List<GroupInstanceStudents> groupInstanceStudentsobjectList)
        {
            GroupInstanceStudents groupInstanceStudentsobject = new GroupInstanceStudents();
             groupInstanceStudentsobjectList = new List<GroupInstanceStudents>();
            foreach (var item in groupInstanceStudentslist)
            {
                groupInstanceStudentsobject = new GroupInstanceStudents();

                foreach (var student in item.Students)
                {
                    groupInstanceStudentsobject.GroupInstanceId = item.GroupInstanceId;
                    groupInstanceStudentsobject.StudentId = student.StudentId;
                    groupInstanceStudentsobject.PromoCodeInstanceId = student.PromoCodeInstanceId;
                    groupInstanceStudentsobject.IsPlacementTest = student.isPlacementTest;
                    groupInstanceStudentsobject.CreatedDate = student.CreationDate;
                    groupInstanceStudentsobject.LastModifiedDate = DateTime.Now;
                    groupInstanceStudentsobject.IsDefault = true;
                    groupInstanceStudentsobjectList.Add(groupInstanceStudentsobject);
                }

            }
            var groupInstanceIds = groupInstanceStudentslist.Select(x => x.GroupInstanceId).ToList();
            var students = groupInstanceStudents.Where(x => groupInstanceIds.Contains(x.GroupInstanceId)).ToList();
            return students;
        }

        public void ValidateGroupInstancesStudents(int groupDefinitionId, List<StudentsGroupInstanceModel> groupInstanceStudentslist)
        {
            var groupdefinition = groupDefinition.Include(x => x.GroupCondition).Where(x => x.Id == groupDefinitionId).FirstOrDefault();
            if (groupDefinition == null)
            {
                throw new Exception("Group Definition Not Found");
            }
            foreach (var groupInstanceStudent in groupInstanceStudentslist)
            {
                int paymentStudents = groupInstanceStudent.Students.Where(x => x.isPlacementTest == false && x.PromoCodeId == null).Count();
                int promocodesStudents = groupInstanceStudent.Students.Where(x => x.isPlacementTest == false && x.PromoCodeId != null).Count();
                int placementStudents = groupInstanceStudent.Students.Where(x => x.isPlacementTest == true && x.PromoCodeId == null).Count();
                if (groupInstanceStudent.Students.Count() > groupdefinition.GroupCondition.NumberOfSlots) // check total students
                {
                    throw new Exception($"Group Instance Serial {groupInstanceStudent.GroupInstanceSerail} must contain {groupdefinition.GroupCondition.NumberOfSlots} student not {groupInstanceStudent.Students.Count()} ");
                }

                if (placementStudents > groupdefinition.GroupCondition.NumberOfSlotsWithPlacementTest) // check placement students
                {
                    throw new Exception($"Group Instance Serial {groupInstanceStudent.GroupInstanceSerail} must contain {groupdefinition.GroupCondition.NumberOfSlotsWithPlacementTest} placement stuident not {placementStudents} ");
                }
                // check promocode students

            }

            GroupInstanceStudents groupInstanceStudentsobject = new GroupInstanceStudents();
            List<GroupInstanceStudents> groupInstanceStudentsobjectList = new List<GroupInstanceStudents>();
            foreach (var item in groupInstanceStudentslist)
            {
                groupInstanceStudentsobject = new GroupInstanceStudents();

                foreach (var student in item.Students)
                {
                    groupInstanceStudentsobject.GroupInstanceId = item.GroupInstanceId;
                    groupInstanceStudentsobject.StudentId = student.StudentId;
                    groupInstanceStudentsobject.PromoCodeInstanceId = student.PromoCodeInstanceId;
                    groupInstanceStudentsobject.IsPlacementTest = student.isPlacementTest;
                    groupInstanceStudentsobject.CreatedDate = student.CreationDate;
                    groupInstanceStudentsobject.LastModifiedDate = DateTime.Now;
                    groupInstanceStudentsobject.IsDefault = true;
                    groupInstanceStudentsobjectList.Add(groupInstanceStudentsobject);
                }

            }
            var groupInstanceIds = groupInstanceStudentslist.Select(x => x.GroupInstanceId).ToList();
            var students = groupInstanceStudents.Where(x => groupInstanceIds.Contains(x.GroupInstanceId)).ToList();
            groupInstanceStudents.RemoveRange(students);
            groupInstanceStudents.AddRange(groupInstanceStudentsobjectList);
        }

        public List<IGrouping<int, GroupInstanceStudents>> GetAllByGroupDefinition(int? groupDefinitionId = null, int? groupInstanceId = null)
        {
            var students = groupInstanceStudents.Where(x =>
                                                        (groupDefinitionId != null ? x.GroupInstance.GroupDefinitionId == groupDefinitionId : true)
                                                        && (groupInstanceId != null ? (x.GroupInstanceId == groupInstanceId) : true)).ToList();
            var groupedStudents = students.GroupBy(x => x.GroupInstanceId).ToList();
            return groupedStudents;
        }

        public List<GroupInstanceStudents> GetByGroupDefinitionAndGroupInstance(int groupDefinitionId,int? groupinstanceId=null)
        {
            var students = groupInstanceStudents.Where(x => x.GroupInstance.GroupDefinitionId == groupDefinitionId 
                                                        && (groupinstanceId!=null?x.GroupInstanceId ==groupinstanceId:true)).ToList();
            return students;
            
        }
    }
}
