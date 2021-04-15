using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class AdlerCardSubmissionRepositoryAsync : GenericRepositoryAsync<AdlerCardSubmission>, IAdlerCardSubmissionRepositoryAsync
    {
        private readonly DbSet<AdlerCardSubmission> _adlercardsubmissions;


        public AdlerCardSubmissionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _adlercardsubmissions = dbContext.Set<AdlerCardSubmission>();

        }

        public AdlerCardSubmission GetAdlerCardForStudent(string studentId, int adlercardId)
        {
            return _adlercardsubmissions.Include(x => x.AdlerCard.Question.SingleQuestions).Include(x => x.AdlerCard.Level).Where(x => x.AdlerCardId == adlercardId && x.StudentId == studentId).FirstOrDefault();
        }

        public IEnumerable<AdlerCardsSubmissionsForStaffModel> GetAdlerCardsSubmissionsForStaff(int pageNumber, int pageSize, string studentId, string studentName, int? levelId, string levelName, int? type, int? status, bool? assigned, string TeacherId, out int totalCount)
        {
            var query = _adlercardsubmissions.Include(x => x.Student).Include(x => x.Teacher).Include(x => x.AdlerCard.AdlerCardsUnit).Include(x => x.AdlerCard.AdlerCardsUnit).Where(x => x.Id >= 0);
            if (!string.IsNullOrEmpty(TeacherId))
            {
                query = query.Where(x => x.TeacherId == TeacherId);
            }
            if (!string.IsNullOrEmpty(studentId))
            {
                query = query.Where(x => x.StudentId == studentId);
            }
            if (!string.IsNullOrEmpty(studentName))
            {
                query = query.Where(x => x.Student.FirstName.ToLower().Contains(studentName.ToLower()) || x.Student.LastName.ToLower().Contains(studentName.ToLower()));
            }
            if (levelId.HasValue)
            {
                query = query.Where(x => x.AdlerCard.LevelId == levelId.Value);
            }
            if (!string.IsNullOrEmpty(levelName))
            {
                query = query.Where(x => x.AdlerCard.Level.Name.ToLower().Contains(levelName.ToLower()));
            }
            if (type.HasValue)
            {
                query = query.Where(x => x.AdlerCard.AdlerCardsTypeId == type.Value);
            }
            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }
            if (assigned.HasValue)
            {
                if (assigned.Value)
                {
                    query = query.Where(x => x.TeacherId != null);
                }
                else
                {
                    query = query.Where(x => x.TeacherId == null);
                }
            }

            totalCount = query.Count();
            var adlerSubmission = query.Select(x => new AdlerCardsSubmissionsForStaffModel()
            {
                AdlerCardsSubmissionsId = x.Id,
                Student = new StudentsModel()
                {
                    StudentId = x.StudentId,
                    StudentName = $"{ x.Student.FirstName} {x.Student.LastName}"
                },
                Unit = x.AdlerCard.AdlerCardsUnit,
                Level = x.AdlerCard.Level,
                status = x.Status.ToString(),
                Type = x.AdlerCard.AdlerCardsTypeId.ToString(),
                Score = x.AchievedScore,
                AssignedTo = x.TeacherId,
                AssignedTeacherName = x.Teacher != null ? $"{x.Teacher.FirstName} {x.Teacher.LastName}" : string.Empty
            }).Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    //.OrderBy(sortBy, sortASC)
                    .AsNoTracking()
                    .ToList();
            return adlerSubmission;
        }
    }

}
