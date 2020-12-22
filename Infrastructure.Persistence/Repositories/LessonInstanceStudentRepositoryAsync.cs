using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class LessonInstanceStudentRepositoryAsync : GenericRepositoryAsync<LessonInstanceStudent>, ILessonInstanceStudentRepositoryAsync
    {
        private readonly DbSet<LessonInstanceStudent> lessonInstanceStudents;
        public LessonInstanceStudentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            lessonInstanceStudents = dbContext.Set<LessonInstanceStudent>();
        }

        public IEnumerable<LessonInstanceStudent> GetStudentsByLessonInstance(int LessonInstanceId)
        {
            return lessonInstanceStudents.Include(x => x.Student).Where(x => x.LessonInstanceId == LessonInstanceId).ToList();
        }
    }
}
