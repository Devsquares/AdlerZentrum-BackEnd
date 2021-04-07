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
    public class ClaimRepositoryAsync : IClaimRepositoryAsync
    {
        private readonly ApplicationDbContext _context;


        public ClaimRepositoryAsync(ApplicationDbContext dbContext) 
        {
            _context = dbContext;

        }

        public bool CheckUserClaims(string userId,string claim)
        {
            var user = _context.UserClaims.Where(x => x.UserId == userId && x.ClaimType.ToLower() == claim.ToLower()).FirstOrDefault();
            if(user != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
       
    }

}
