using System.Collections.Generic;
using System.Linq;
using Application.Enums;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class HomeWorkRepositoryAsync : GenericRepositoryAsync<Homework>, IHomeWorkRepositoryAsync
    {
        private readonly DbSet<Homework> homework;
        public HomeWorkRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            homework = dbContext.Set<Homework>();
        }

        public ICollection<Homework> GetAllBounsRequests(int pageNumber, int pageSize, int? status)
        {
            var homeworks = new List<Homework>();
            if (status == null)
            {
                homeworks = homework
                               .Include(x => x.Teacher)
                               .Include(x => x.LessonInstance)
                               .Include(x => x.GroupInstance)
                               .Include(x => x.GroupInstance.GroupDefinition)
                               .Include(x => x.GroupInstance.GroupDefinition.Sublevel)
                               .Where(x => x.BonusPoints > 0 && (status == null ? true : x.BonusPointsStatus == (int)status))
                               .OrderBy(x => x.BonusPointsStatus)
                               .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                homeworks = homework
                               .Include(x => x.Teacher)
                               .Include(x => x.LessonInstance)
                               .Include(x => x.GroupInstance)
                               .Include(x => x.GroupInstance.GroupDefinition)
                               .Include(x => x.GroupInstance.GroupDefinition.Sublevel)
                               .Where(x => x.BonusPoints > 0 && (status == null ? true : x.BonusPointsStatus == (int)status))
                               .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            return homeworks;
        }

        public int GetAllBounsRequestsCount(int? status)
        {
            return homework
                    .Include(x => x.Teacher)
                    .Include(x => x.LessonInstance)
                    .Include(x => x.GroupInstance)
                    .Include(x => x.GroupInstance.GroupDefinition)
                    .Include(x => x.GroupInstance.GroupDefinition.Sublevel)
                    .Where(x => x.BonusPoints > 0 && (status == null ? true : x.BonusPointsStatus == (int)status)).Count();
        }

        public Homework GetByLessonInstance(int LessonInstanceId)
        {
            return homework.Where(x => x.LessonInstanceId == LessonInstanceId).FirstOrDefault();
        }
    }
}
