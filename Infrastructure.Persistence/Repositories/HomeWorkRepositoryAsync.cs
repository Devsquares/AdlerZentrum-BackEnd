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
    }
}