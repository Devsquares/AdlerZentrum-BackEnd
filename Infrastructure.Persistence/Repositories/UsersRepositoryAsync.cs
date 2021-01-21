using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    class UsersRepositoryAsync : GenericRepositoryAsync<ApplicationUser>, IUsersRepositoryAsync
    {
        private readonly DbSet<ApplicationUser> users;

        public UsersRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            users = dbContext.Set<ApplicationUser>();
        }

        public ApplicationUser GetUserById(string Id)
        {
            return users.Where(x => x.Id == Id).FirstOrDefault();
        }
    }
}
