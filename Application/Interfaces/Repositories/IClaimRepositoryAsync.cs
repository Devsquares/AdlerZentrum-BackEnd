using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IClaimRepositoryAsync 
    {
        bool CheckUserClaims(string userId, string claim);
    }
}
