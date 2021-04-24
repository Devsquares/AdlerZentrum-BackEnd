using Application.Enums;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Persistence.Repositories
{
    public class TeacherGroupInstanceAssignmentRepositoryAsync : GenericRepositoryAsync<TeacherGroupInstanceAssignment>, ITeacherGroupInstanceAssignmentRepositoryAsync
    {
        private readonly DbSet<TeacherGroupInstanceAssignment> groupInstances;
        private readonly ApplicationDbContext _dbContext;
        public TeacherGroupInstanceAssignmentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            groupInstances = dbContext.Set<TeacherGroupInstanceAssignment>();
            _dbContext = dbContext;
        }

        public IEnumerable<TeacherGroupInstanceAssignment> GetByTeacher(string TeacherId)
        {
            return groupInstances.Include(x => x.GroupInstance).Where(x => x.TeacherId == TeacherId);
        }

        public TeacherGroupInstanceAssignment GetByTeacherId(string TeacherId)
        {
            return groupInstances.Include(x => x.GroupInstance).Where(x => x.TeacherId == TeacherId).FirstOrDefault();
        }


        public TeacherGroupInstanceAssignment GetByGroupInstanceId(int groupInstanceId)
        {
            return groupInstances.Include(x => x.Teacher).Where(x => x.GroupInstanceId == groupInstanceId && x.IsDefault == true).FirstOrDefault();
        }
        public TeacherGroupInstanceAssignment GetByTeachGroupInstanceId(string TeacherId, int groupInstanceId)
        {
            return groupInstances.Where(x => x.GroupInstanceId == groupInstanceId && x.TeacherId == TeacherId).FirstOrDefault();
        }
        public List<TeacherGroupInstanceAssignment> GetListByGroupInstanceId(int groupInstanceId)
        {
            return groupInstances.Include(x => x.Teacher).Where(x => x.GroupInstanceId == groupInstanceId).ToList();
        }

        public List<TeacherGroupInstanceAssignment> GetAll(int pageNumber, int pageSize, out int totalCount, int? sublevelId = null, int? groupDefinationId = null)
        {
            var query = groupInstances.Include(x => x.GroupInstance)
             .Where(x => (sublevelId != null ? x.GroupInstance.GroupDefinition.SubLevelId == sublevelId.Value : true)
              && (groupDefinationId != null ? x.GroupInstance.GroupDefinitionId == groupDefinationId.Value : true))
              .AsQueryable();
            totalCount = query.Count();
            var list = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return list;
        }

        public TeacherGroupInstanceAssignment GetFirstNotIsDefault(int groupInstanceId)
        {
            return groupInstances.Include(x => x.Teacher).Where(x => x.GroupInstanceId == groupInstanceId && x.IsDefault == false).FirstOrDefault();
        }
        public TeacherGroupInstanceAssignment GetByGroupInstanceIdWithoutDefault(int groupInstanceId)
        {
            return groupInstances.Include(x => x.Teacher).Where(x => x.GroupInstanceId == groupInstanceId).FirstOrDefault();
        }

        public List<TeacherAnalysisReportModel> GetTeacherAnalysisReport(int pageNumber,int PageSize,string teacherName, DateTime? from, DateTime? to
            , int? homeworksUploadDelayFrom, int? homeworksUploadDelayTo, int? homeworksCorrectionDelayFrom, int? homeworksCorrectionDelayTo,
            int? testsCorrectionDelayFrom, int? testsCorrectionDelayTo, int? feedbackScoreFrom, int? feedbackScoreto,out int count)
        {
            var userListquery = (from user in _dbContext.ApplicationUsers
                                 join userroles in _dbContext.UserRoles
                                 on user.Id equals userroles.UserId
                                 join roles in _dbContext.Roles
                                 on userroles.RoleId equals roles.Id
                                 where roles.Name.ToLower() == "teacher"
                                 select new
                                 {
                                     userId = user.Id,
                                     UserName = $"{user.FirstName} {user.LastName}"
                                 }
                         ).ToList();
            List<TeacherAnalysisReportModel> teacherAnalysisReport = new List<TeacherAnalysisReportModel>();
            TeacherAnalysisReportModel teacherAnalysisReportobject = new TeacherAnalysisReportModel();
            for (int i = 0; i < userListquery.Count(); i++)
            {
                teacherAnalysisReportobject = new TeacherAnalysisReportModel();
                teacherAnalysisReportobject.TeacherName = userListquery[i].UserName;
                var lessons = _dbContext.Homeworks.Where(x => x.TeacherId == userListquery[i].userId).ToList().GroupBy(x => x.LessonInstanceId).ToDictionary(y => y.Key, y => y.Select(z => z).Count());
                double homeworksCount = 0;
                if (lessons != null && lessons.Count > 0)
                {
                    foreach (var item in lessons)
                    {
                        homeworksCount += item.Value;
                    }
                    teacherAnalysisReportobject.HomeworksUpload =Math.Round( (homeworksCount / lessons.Count()),2);
                }

                var homeworkSub = _dbContext.HomeWorkSubmitions.Where(x => x.CorrectionTeacherId == userListquery[i].userId && x.Status == (int)HomeWorkSubmitionStatusEnum.Corrected && x.CorrectionDate != null && x.CorrectionDueDate !=null);
                if(from != null && to!=null)
                {
                    homeworkSub = homeworkSub.Where(x => x.CorrectionDueDate >= from && x.CorrectionDueDate <= to);
                }
                var homeworkSubResult = homeworkSub.ToList();
                double homeworkDelayTotalHours = 0;
                foreach (var item in homeworkSubResult)
                {
                    homeworkDelayTotalHours += (item.CorrectionDueDate - item.CorrectionDate).Value.TotalHours;
                }
                teacherAnalysisReportobject.HomeworksCorrectionDelay = Math.Round(homeworkDelayTotalHours,2);
                //lessons
                var lessonsinstance = _dbContext.LessonInstances.Where(x => x.SubmittedReportTeacherId == userListquery[i].userId &&  x.DueDate != null && x.SubmissionDate != null);
                if (from != null && to != null)
                {
                    lessonsinstance = lessonsinstance.Where(x => x.DueDate >= from && x.DueDate <= to);
                }
                var lessonsinstanceResult = lessonsinstance.ToList();
                double homeworkUploadDelayTotalHours = 0;
                foreach (var item in lessonsinstanceResult)
                {
                    homeworkUploadDelayTotalHours += (item.DueDate - item.SubmissionDate).Value.TotalHours;
                }
                teacherAnalysisReportobject.HomeworksUploadDelay = Math.Round(homeworkUploadDelayTotalHours,2);
                //test
                var tests = _dbContext.TestInstances.Where(x => x.CorrectionTeacherId == userListquery[i].userId && x.CorrectionDueDate != null && x.CorrectionDate != null);
                if (from != null && to != null)
                {
                    tests = tests.Where(x => x.CorrectionDueDate >= from && x.CorrectionDueDate <= to);
                }
                var testsResult = tests.ToList();
                double testsCorrectionDelayTotalHours = 0;
                foreach (var item in testsResult)
                {
                    testsCorrectionDelayTotalHours += (item.CorrectionDueDate - item.CorrectionDate).TotalHours;
                }
                teacherAnalysisReportobject.TestsCorrectionDelay = Math.Round(testsCorrectionDelayTotalHours,2);

                teacherAnalysisReport.Add(teacherAnalysisReportobject);
            }
            if(!string.IsNullOrEmpty(teacherName))
            {
                teacherAnalysisReport = teacherAnalysisReport.Where(x => x.TeacherName.ToLower().Contains(teacherName)).ToList();
            }
            if(homeworksUploadDelayFrom !=null && homeworksUploadDelayTo != null)
            {
                teacherAnalysisReport = teacherAnalysisReport.Where(x => x.HomeworksUploadDelay >= homeworksUploadDelayFrom && x.HomeworksUploadDelay <= homeworksUploadDelayTo).ToList();
            }
            if (homeworksCorrectionDelayFrom != null && homeworksCorrectionDelayTo != null)
            {
                teacherAnalysisReport = teacherAnalysisReport.Where(x => x.HomeworksCorrectionDelay >= homeworksCorrectionDelayFrom && x.HomeworksCorrectionDelay <= homeworksCorrectionDelayTo).ToList();
            }
            if (testsCorrectionDelayFrom != null && testsCorrectionDelayTo != null)
            {
                teacherAnalysisReport = teacherAnalysisReport.Where(x => x.TestsCorrectionDelay >= testsCorrectionDelayFrom && x.TestsCorrectionDelay <= testsCorrectionDelayTo).ToList();
            }
            count = teacherAnalysisReport.Count();
            return teacherAnalysisReport.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();
           
        }
    }
}
