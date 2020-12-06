using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class HomeWorkSubmitionRepositoryAsync : GenericRepositoryAsync<HomeWorkSubmition>, IHomeWorkSubmitionRepositoryAsync
    {
        private readonly DbSet<HomeWorkSubmition> homeWorkSubmitions;
        public HomeWorkSubmitionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            homeWorkSubmitions = dbContext.Set<HomeWorkSubmition>();
        }
    }
}
