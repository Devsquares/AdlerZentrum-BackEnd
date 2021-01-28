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

        public List<StudentsGroupInstanceModel> GetListByGroupDefinitionId(int groupDefinitionId, List<int> groupInstancelist = null)
        {
            List<StudentsGroupInstanceModel> StudentsGroupInstanceModelList = new List<StudentsGroupInstanceModel>();
            StudentsGroupInstanceModel StudentsGroupInstanceObject = new StudentsGroupInstanceModel();
            var groupInstanceList = groupInstances.Where(x => x.GroupDefinitionId == groupDefinitionId
            && ((groupInstancelist != null && groupInstancelist.Count > 0) ? (groupInstancelist.Contains(x.Id)) : true)
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
        public int GetCountByGroupDefinitionId(int groupDefinitionId)
        {
            return groupInstances.Where(x => x.GroupDefinitionId == groupDefinitionId).Count();
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
            List<GroupInstanceStudents> interestedGroupInstanceStudents = new List<GroupInstanceStudents>();
            // Add interested with  promoCOdes
            if (interestedStudentsList != null && interestedStudentsList.Count > 0)
            {

                foreach (var interestedStudent in interestedStudentsList)
                {
                    canApplyInSpecificGroup = _groupConditionPromoCodeRepositoryAsync.CheckPromoCodeCountInGroupInstance(groupInstanceobject.Id, interestedStudent.PromoCodeId);
                    if (canApplyInSpecificGroup && studentCount < totalStudents)
                    {
                        interestedGroupInstanceStudents.Add(new GroupInstanceStudents
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
                if (interestedGroupInstanceStudents != null && interestedGroupInstanceStudents.Count > 0)
                {
                    await _groupInstanceStudentRepositoryAsync.AddBulkAsync(interestedGroupInstanceStudents);
                    InterestedStudents.RemoveRange(interestedStudentsList);
                }
            }

            List<GroupInstanceStudents> OverPaymentsGroupInstanceStudents = new List<GroupInstanceStudents>();
            // Add overpayment students
            if (overPaymentStudentList != null && overPaymentStudentList.Count > 0)
            {
                foreach (var overPaymentStudent in overPaymentStudentList)
                {
                    if (studentCount < totalStudents)
                    {
                        OverPaymentsGroupInstanceStudents.Add(new GroupInstanceStudents
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
                if (OverPaymentsGroupInstanceStudents != null && OverPaymentsGroupInstanceStudents.Count > 0)
                {
                    await _groupInstanceStudentRepositoryAsync.AddBulkAsync(OverPaymentsGroupInstanceStudents);
                    OverPaymentStudents.RemoveRange(overPaymentStudentList);
                }
            }

            if (studentCount == totalStudents)
            {
                groupInstanceobject.Status = (int)GroupInstanceStatusEnum.SlotCompleted;
                groupInstances.Update(groupInstanceobject);
            }
            List<int> groupInstanceId = new List<int>();
            groupInstanceId.Add(groupInstanceobject.Id);
            var groupInstanceStudents = GetListByGroupDefinitionId(groupDefinitionId, groupInstanceId);
            return (groupInstanceStudents != null && groupInstanceStudents.Count > 0) ? groupInstanceStudents[0] : null;
        }

        public async Task<List<StudentsGroupInstanceModel>> EditGroupByAddStudentFromAnotherGroup(int groupDefinitionId, int srcGroupInstanceId, int desGroupInstanceId, string studentId, int? promoCodeId)
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
            var sourceGroupInstance = groupInstances.Where(x => x.Id == srcGroupInstanceId && (x.Status == (int)GroupInstanceStatusEnum.Pending || x.Status == (int)GroupInstanceStatusEnum.SlotCompleted)).FirstOrDefault();
            if (sourceGroupInstance == null)
            {
                throw new ApiException($"You cannot EDit as Source group instance {((GroupInstanceStatusEnum)sourceGroupInstance.Status).ToString()}");
            }
            var destinationGroupInstance = groupInstances.Where(x => x.Id == desGroupInstanceId && (x.Status == (int)GroupInstanceStatusEnum.Pending || x.Status == (int)GroupInstanceStatusEnum.SlotCompleted)).FirstOrDefault();
            if (destinationGroupInstance == null)
            {
                throw new ApiException($"You cannot EDit as Destination group instance {((GroupInstanceStatusEnum)destinationGroupInstance.Status).ToString()}");
            }
            bool canApplyInSpecificGroup = false;
            int totalStudents = groupDefinitionobject.GroupCondition.NumberOfSlots;
            int desTotalStudent = _groupInstanceStudentRepositoryAsync.GetCountOfStudents(desGroupInstanceId);
            if (desTotalStudent == totalStudents)
            {
                throw new ApiException($"you cann't add student to the desyination group instance as it is full");
            }
            var student = _groupInstanceStudentRepositoryAsync.GetByStudentId(studentId, sourceGroupInstance.Id);
            if (promoCodeId != null)
            {
                canApplyInSpecificGroup = _groupConditionPromoCodeRepositoryAsync.CheckPromoCodeCountInGroupInstance(destinationGroupInstance.Id, promoCodeId.Value);
                if (canApplyInSpecificGroup)
                {
                    student.GroupInstanceId = destinationGroupInstance.Id;
                    await _groupInstanceStudentRepositoryAsync.UpdateAsync(student);
                }
            }
            else
            {
                student.GroupInstanceId = destinationGroupInstance.Id;
                await _groupInstanceStudentRepositoryAsync.UpdateAsync(student);
            }

            if (sourceGroupInstance.Status == (int)GroupInstanceStatusEnum.SlotCompleted)
            {
                sourceGroupInstance.Status = (int)GroupInstanceStatusEnum.Pending;
                groupInstances.Update(sourceGroupInstance);
            }
            desTotalStudent = _groupInstanceStudentRepositoryAsync.GetCountOfStudents(desGroupInstanceId);
            if (desTotalStudent == totalStudents)
            {
                destinationGroupInstance.Status = (int)GroupInstanceStatusEnum.SlotCompleted;
                groupInstances.Update(destinationGroupInstance);
            }
            List<int> groupInstanceId = new List<int>();
            groupInstanceId.Add(srcGroupInstanceId);
            groupInstanceId.Add(desGroupInstanceId);
            var studentsGroup = GetListByGroupDefinitionId(groupDefinitionId, groupInstanceId);
            return studentsGroup;
        }
        public async Task<List<StudentsGroupInstanceModel>> SwapStudentBetweenTwoGroups(int groupDefinitionId, int srcGroupInstanceId, string srcstudentId, int desGroupInstanceId, string desstudentId)
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
            var sourceGroupInstance = groupInstances.Where(x => x.Id == srcGroupInstanceId && (x.Status == (int)GroupInstanceStatusEnum.Pending || x.Status == (int)GroupInstanceStatusEnum.SlotCompleted)).FirstOrDefault();
            if (sourceGroupInstance == null)
            {
                throw new ApiException($"You cannot EDit as Source group instance {((GroupInstanceStatusEnum)sourceGroupInstance.Status).ToString()}");
            }
            var destinationGroupInstance = groupInstances.Where(x => x.Id == desGroupInstanceId && (x.Status == (int)GroupInstanceStatusEnum.Pending || x.Status == (int)GroupInstanceStatusEnum.SlotCompleted)).FirstOrDefault();
            if (destinationGroupInstance == null)
            {
                throw new ApiException($"You cannot EDit as Destination group instance {((GroupInstanceStatusEnum)destinationGroupInstance.Status).ToString()}");
            }
            var sourcestudent = _groupInstanceStudentRepositoryAsync.GetByStudentId(srcstudentId, sourceGroupInstance.Id);
            var destinationstudent = _groupInstanceStudentRepositoryAsync.GetByStudentId(desstudentId, destinationGroupInstance.Id);
            sourcestudent.GroupInstanceId = destinationGroupInstance.Id;
            destinationstudent.GroupInstanceId = sourceGroupInstance.Id;
            List<GroupInstanceStudents> swapstudents = new List<GroupInstanceStudents>();
            swapstudents.Add(sourcestudent);
            swapstudents.Add(destinationstudent);
            await _groupInstanceStudentRepositoryAsync.UpdateBulkAsync(swapstudents);
            List<int> groupInstanceId = new List<int>();
            groupInstanceId.Add(srcGroupInstanceId);
            groupInstanceId.Add(desGroupInstanceId);
            var studentsGroup = GetListByGroupDefinitionId(groupDefinitionId, groupInstanceId);
            return studentsGroup;
        }
    }
}
