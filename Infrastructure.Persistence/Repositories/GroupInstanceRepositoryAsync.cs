﻿using Application.Enums;
using Application.Exceptions;
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
    public class GroupInstanceRepositoryAsync : GenericRepositoryAsync<GroupInstance>, IGroupInstanceRepositoryAsync
    {
        private readonly DbSet<GroupInstance> groupInstances;
        private readonly DbSet<GroupInstanceStudents> groupInstanceStudents;
        private readonly DbSet<InterestedStudent> InterestedStudents;
        private readonly DbSet<OverPaymentStudent> OverPaymentStudents;
        private readonly IGroupConditionPromoCodeRepositoryAsync _groupConditionPromoCodeRepositoryAsync;
        private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
        private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepositoryAsync;
        private readonly DbSet<TeacherGroupInstanceAssignment> teacherGroupInstanceAssignment;
        public GroupInstanceRepositoryAsync(ApplicationDbContext dbContext,
            IGroupConditionPromoCodeRepositoryAsync groupConditionPromoCodeRepositoryAsync,
             IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
             IGroupDefinitionRepositoryAsync GroupDefinitionRepository) : base(dbContext)

        {
            groupInstances = dbContext.Set<GroupInstance>();
            groupInstanceStudents = dbContext.Set<GroupInstanceStudents>();
            InterestedStudents = dbContext.Set<InterestedStudent>();
            OverPaymentStudents = dbContext.Set<OverPaymentStudent>();
            _groupConditionPromoCodeRepositoryAsync = groupConditionPromoCodeRepositoryAsync;
            _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
            _GroupDefinitionRepositoryAsync = GroupDefinitionRepository;
            teacherGroupInstanceAssignment = dbContext.Set<TeacherGroupInstanceAssignment>();
        }

        public int? GetActiveGroupInstance(string userId)
        {
            return groupInstanceStudents.Where(x => x.StudentId == userId && x.IsDefault == true).FirstOrDefault()?.GroupInstanceId;
        }

        public override async Task<IReadOnlyList<GroupInstance>> GetPagedReponseAsync(FilteredRequestParameter filteredRequestParameter)
        {
            bool noPaging = filteredRequestParameter.NoPaging;
            if (noPaging)
            {
                filteredRequestParameter.PageNumber = 1;
                filteredRequestParameter.PageSize = FilteredRequestParameter.MAX_ELEMENTS;
            }
            int pageNumber = filteredRequestParameter.PageNumber;
            int pageSize = filteredRequestParameter.PageSize;

            string sortBy = filteredRequestParameter.SortBy;
            if (sortBy == null)
            {
                sortBy = "ID";
            }
            string sortType = filteredRequestParameter.SortType;
            bool sortASC = true;

            if (sortType.ToUpper().Equals("DESC"))
            {
                sortASC = false;
            }
            return await groupInstances
                .Include(x => x.GroupDefinition)
                .Include(x => x.GroupDefinition.GroupCondition)
                .Include(x => x.GroupDefinition.TimeSlot)
                .Include(x => x.GroupDefinition.Sublevel)
                .Include(x => x.GroupDefinition.TimeSlot.TimeSlotDetails)
                .Where(IsMatchedExpression(filteredRequestParameter))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    //.OrderBy(sortBy, sortASC)
                    .AsNoTracking()
                    .ToListAsync();

        }

        public IReadOnlyList<GroupInstanceStudents> GetStudents(int groupId)
        {
            return groupInstances.Include(x => x.Students).Where(x => x.Id == groupId).FirstOrDefault()?.Students.ToList();
        }

        public async void AddStudentToTheGroupInstance(int groupId, string studentId)
        {
            await groupInstanceStudents.AddAsync(new GroupInstanceStudents
            {
                GroupInstanceId = groupId,
                IsDefault = true,
                StudentId = studentId
            });
        }

        public async Task<GroupInstance> GetByIdAsync(int id)
        {
            return groupInstances
                  .Include(x => x.GroupDefinition)
                  .Include(x => x.Students)
                  .Include(x => x.GroupDefinition.TimeSlot)
                  .Include(x => x.GroupDefinition.Sublevel)
                  .Include(x => x.GroupDefinition.Sublevel.LessonDefinitions)
                  .Where(x => x.Id == id).FirstOrDefault();
        }

        public GroupInstance GetByGroupDefinitionId(int groupDefinitionId)
        {
            return groupInstances
                  .Include(x => x.GroupDefinition.GroupCondition)
                  .Include(x => x.Students)
                  .Include(x => x.GroupDefinition.TimeSlot)
                  .Include(x => x.GroupDefinition.Sublevel)
                  .Include(x => x.GroupDefinition.Sublevel.LessonDefinitions)
                  .Where(x => x.GroupDefinitionId == groupDefinitionId && x.Status == (int)GroupInstanceStatusEnum.Pending).FirstOrDefault();
        }

        public List<StudentsGroupInstanceModel> GetListByGroupDefinitionId(int groupDefinitionId, List<int> groupInstancelist = null)
        {
            List<StudentsGroupInstanceModel> StudentsGroupInstanceModelList = new List<StudentsGroupInstanceModel>();
            StudentsGroupInstanceModel StudentsGroupInstanceObject = new StudentsGroupInstanceModel();
            var groupInstanceList = groupInstances.Where(x => x.GroupDefinitionId == groupDefinitionId
            && ((groupInstancelist != null && groupInstancelist.Count > 0) ? (groupInstancelist.Contains(x.Id)) : true)
            && (x.Status == (int)GroupInstanceStatusEnum.Pending || x.Status == (int)GroupInstanceStatusEnum.SlotCompleted)).OrderByDescending(x => x.Id).ToList();
            foreach (var groupInstance in groupInstanceList)
            {
                StudentsGroupInstanceObject = new StudentsGroupInstanceModel();
                StudentsGroupInstanceObject.GroupInstanceId = groupInstance.Id;
                StudentsGroupInstanceObject.GroupInstanceSerail = groupInstance.Serial;
                StudentsGroupInstanceObject.Status = ((GroupInstanceStatusEnum)groupInstance.Status).ToString();
                StudentsGroupInstanceObject.Students = groupInstanceStudents.Include(x => x.PromoCode).Include(x => x.Student).Where(x => x.GroupInstanceId == groupInstance.Id).Select(x => new StudentsModel
                {
                    StudentId = x.StudentId,
                    StudentName = $"{x.Student.FirstName} {x.Student.LastName}",
                    PromoCodeId = x.PromoCodeId,
                    PromoCodeName = x.PromoCode != null ? x.PromoCode.Name : string.Empty,
                    isPlacementTest = x.IsPlacementTest,
                    CreationDate = x.CreatedDate

                }).ToList();
                StudentsGroupInstanceObject.Teachers = new List<TeachersModel>();

                var teacher = teacherGroupInstanceAssignment.Include(x => x.Teacher).Where(x => x.GroupInstanceId == groupInstance.Id).Select(x => x.Teacher).FirstOrDefault();
                if (teacher != null)
                {
                    StudentsGroupInstanceObject.Teachers.Add(new TeachersModel
                    {
                        TeacherFirstName = teacher.FirstName,
                        TeacherLastName = teacher.LastName,
                        TeacherId = teacher.Id

                    });
                }


                StudentsGroupInstanceModelList.Add(StudentsGroupInstanceObject);
            }
            return StudentsGroupInstanceModelList;
        }
        public int GetCountByGroupDefinitionId(int groupDefinitionId)
        {
            return groupInstances.Where(x => x.GroupDefinitionId == groupDefinitionId).Count();
        }
        public async Task<GroupInstance> GetByIdPendingorCompleteAsync(int id)
        {
            return groupInstances
                  .Include(x => x.GroupDefinition)
                  .Include(x => x.Students)
                  .Include(x => x.GroupDefinition.TimeSlot)
                  .Include(x => x.GroupDefinition.Sublevel)
                  .Include(x => x.GroupDefinition.Sublevel.LessonDefinitions)
                  .Where(x => x.Id == id && (x.Status == (int)GroupInstanceStatusEnum.Pending || x.Status == (int)GroupInstanceStatusEnum.SlotCompleted)).FirstOrDefault();
        }

    }
}
