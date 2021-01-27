using Application.Enums;
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

        public List<StudentsGroupInstanceModel> GetListByGroupDefinitionId(int groupDefinitionId, int? groupInstanceId = null)
        {
            List<StudentsGroupInstanceModel> StudentsGroupInstanceModelList = new List<StudentsGroupInstanceModel>();
            StudentsGroupInstanceModel StudentsGroupInstanceObject = new StudentsGroupInstanceModel();
            var groupInstanceList = groupInstances.Where(x => x.GroupDefinitionId == groupDefinitionId
            && (groupInstanceId != null ? (x.Id == groupInstanceId) : true)
            && (x.Status == (int)GroupInstanceStatusEnum.Pending || x.Status == (int)GroupInstanceStatusEnum.SlotCompleted)).OrderByDescending(x => x.Id).ToList();
            foreach (var groupInstance in groupInstanceList)
            {
                StudentsGroupInstanceObject.GroupInstanceId = groupInstance.Id;
                StudentsGroupInstanceObject.Status = ((GroupInstanceStatusEnum)groupInstance.Status).ToString();
                StudentsGroupInstanceObject.Students = groupInstanceStudents.Include(x => x.PromoCode).Include(x => x.Student).Where(x => x.GroupInstanceId == groupInstance.Id).Select(x => new StudentsModel
                {
                    StudentId = x.StudentId,
                    StudentName = $"{x.Student.FirstName} {x.Student.LastName}",
                    PromoCodeId = x.PromoCodeId,
                    PromoCodeName = x.PromoCode != null ? x.PromoCode.Name : string.Empty

                }).ToList();
                StudentsGroupInstanceObject.Teachername = teacherGroupInstanceAssignment.Include(x => x.Teacher).Where(x => x.GroupInstanceId == groupInstance.Id).Select(x => x.Teacher.FirstName).FirstOrDefault();
                StudentsGroupInstanceModelList.Add(StudentsGroupInstanceObject);
            }
            return StudentsGroupInstanceModelList;
        }

        public async Task<StudentsGroupInstanceModel> CreateGroupFromInterestedOverPayment(int groupDefinitionId)
        {
            var groupDefinitionobject = await _GroupDefinitionRepositoryAsync.GetByIdAsync(groupDefinitionId);
            if (groupDefinitionobject != null)
            {
                throw new ApiException($"Group definition Not Found");
            }
            if (groupDefinitionobject.Status == (int)GroupDefinationStatusEnum.Finished || groupDefinitionobject.Status == (int)GroupDefinationStatusEnum.Canceld)
            {
                throw new ApiException($"Group definition finished or canceled");
            }
            GroupInstance groupInstanceobject = new GroupInstance()
            {
                GroupDefinitionId = groupDefinitionId,
                CreatedDate = DateTime.Now,
                Status = (int)GroupInstanceStatusEnum.Pending
            };
            await groupInstances.AddAsync(groupInstanceobject);

            var interestedStudentsList = InterestedStudents.Where(x => x.GroupDefinitionId == groupDefinitionId).OrderBy(x => x.Id).ToList();
            var overPaymentStudentList = OverPaymentStudents.Where(x => x.GroupDefinitionId == groupDefinitionId).OrderBy(x => x.Id).ToList();
            bool canApplyInSpecificGroup = false;
            int studentCount = 0;
            int totalStudents = groupDefinitionobject.GroupCondition.NumberOfSlots;
            // Add interested with  promoCOdes
            if (interestedStudentsList != null && interestedStudentsList.Count > 0)
            {
                foreach (var interestedStudent in interestedStudentsList)
                {
                    canApplyInSpecificGroup = _groupConditionPromoCodeRepositoryAsync.CheckPromoCodeCountInGroupInstance(groupInstanceobject.Id, interestedStudent.PromoCodeId);
                    if (canApplyInSpecificGroup && studentCount < totalStudents)
                    {
                        await _groupInstanceStudentRepositoryAsync.AddAsync(new GroupInstanceStudents
                        {
                            GroupInstanceId = groupInstanceobject.Id,
                            StudentId = interestedStudent.StudentId,
                            PromoCodeId = interestedStudent.PromoCodeId,
                            IsDefault = true
                        });
                        studentCount++;
                    }
                    else if (studentCount == totalStudents)
                    {
                        break;
                    }
                }

            }
            // Add overpayment students
            if (overPaymentStudentList != null && overPaymentStudentList.Count > 0)
            {
                foreach (var overPaymentStudent in overPaymentStudentList)
                {
                    if (studentCount < totalStudents)
                    {
                        await _groupInstanceStudentRepositoryAsync.AddAsync(new GroupInstanceStudents
                        {
                            GroupInstanceId = groupInstanceobject.Id,
                            StudentId = overPaymentStudent.StudentId,
                            IsDefault = true
                        });
                        studentCount++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (studentCount == totalStudents)
            {
                groupInstanceobject.Status = (int)GroupInstanceStatusEnum.SlotCompleted;
                groupInstances.Update(groupInstanceobject);
            }
            var groupInstanceStudents = GetListByGroupDefinitionId(groupDefinitionId, groupInstanceobject.Id);
            return (groupInstanceStudents != null && groupInstanceStudents.Count > 0) ? groupInstanceStudents[0] : null;
        }
    }
}
