using Application.Enums;
using Application.Features;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Infrastructure.Persistence.Repositories
{
    public class HomeWorkSubmitionRepositoryAsync : GenericRepositoryAsync<HomeWorkSubmition>, IHomeWorkSubmitionRepositoryAsync
    {
        private readonly DbSet<HomeWorkSubmition> homeWorkSubmitions;
        public HomeWorkSubmitionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            homeWorkSubmitions = dbContext.Set<HomeWorkSubmition>();
        }

        public async Task<IReadOnlyList<HomeWorkSubmition>> GetAllByGroupInstanceAsync(int groupInstanceId, int? Status)
        {
            return await homeWorkSubmitions
                .Include(x => x.Homework)
                .Include(x => x.Student)
                .Include(x => x.Homework.GroupInstance)
                .Include(x => x.Homework.LessonInstance)
            .Where(x => x.Homework.GroupInstanceId == groupInstanceId && Status != null ? x.Status == Status : true).ToListAsync();
        }

        public async Task<IReadOnlyList<HomeWorkSubmition>> GetAllByTeacherIdAsync(string TeacherId, int? Status)
        {
            return await homeWorkSubmitions
                .Include(x => x.Homework)
                .Include(x => x.Student)
                .Include(x => x.Homework.GroupInstance)
                .Include(x => x.Homework.LessonInstance)
                .Where(x => TeacherId != null ? x.Homework.TeacherId == TeacherId : true
                 && Status != null ? x.Status == Status : true)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<HomeWorkSubmition>> GetAllByLessonIdAsync(int lessonId, int? Status)
        {
            return await homeWorkSubmitions
                .Include(x => x.Homework)
                .Include(x => x.Student)
                .Include(x => x.Homework.GroupInstance)
                .Include(x => x.Homework.LessonInstance)
                .Where(x => x.Homework.LessonInstanceId == lessonId
                 && Status != null ? x.Status == Status : true)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<HomeWorkSubmition>> GetAllForStudentAsync(string studentId, int groupInstanceId)
        {
            return await homeWorkSubmitions
                .Include(x => x.Homework)
                .Include(x => x.Homework.LessonInstance)
             .Where(x => x.StudentId == studentId && x.Homework.GroupInstanceId == groupInstanceId).ToListAsync();
        }
        public async Task<HomeWorkSubmition> GetByIdAsync(int id)
        {
            return await homeWorkSubmitions.Include(x => x.Homework.LessonInstance).Include(x => x.Student).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public int GetLateSubmissionsCount(string TeacherName, bool DelaySeen)
        {
            return homeWorkSubmitions.Where(x => (x.CorrectionDate == null && x.CorrectionDueDate < DateTime.Now) || x.CorrectionDate > x.CorrectionDueDate
              && x.DelaySeen == DelaySeen && String.IsNullOrEmpty(TeacherName) ? true :
       (x.CorrectionTeacher.FirstName.Contains(TeacherName) || x.CorrectionTeacher.LastName.Contains(TeacherName))).Count();
        }

        public async Task<List<LateSubmissionsViewModel>> GetLateSubmissions(string TeacherName, int pageNumber, int pageSize, bool DelaySeen)
        {
            //(x.CorrectionTeacher.FirstName.Contains(TeacherName) || x.CorrectionTeacher.LastName.Contains(TeacherName))
            return await homeWorkSubmitions.Where(x => (x.CorrectionDate == null && x.CorrectionDueDate < DateTime.Now) || x.CorrectionDate > x.CorrectionDueDate
                  && x.DelaySeen == DelaySeen && String.IsNullOrEmpty(TeacherName) ? true :
          (x.CorrectionTeacher.FirstName.Contains(TeacherName) || x.CorrectionTeacher.LastName.Contains(TeacherName))
          )
              .Select(x => new LateSubmissionsViewModel()
              {
                  Id = x.Id,
                  Teacher = x.CorrectionTeacher,
                  SubmissionDate = x.CorrectionDate.HasValue ? x.CorrectionDate : null,
                  ExpectedDate = x.CorrectionDueDate.HasValue ? x.CorrectionDueDate : null,
                  DelayDuration = x.CorrectionDate.HasValue ? (x.CorrectionDate.Value - x.CorrectionDueDate.Value).Hours : (x.CorrectionDueDate.Value - DateTime.Now).Hours,
                  homeworkSubmission = x
              })
              .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
