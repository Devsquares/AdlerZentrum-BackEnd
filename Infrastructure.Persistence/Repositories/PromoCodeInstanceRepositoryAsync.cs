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
    public class PromoCodeInstanceRepositoryAsync : GenericRepositoryAsync<PromoCodeInstance>, IPromoCodeInstanceRepositoryAsync
    {
        private readonly DbSet<PromoCodeInstance> _promocodeinstances;


        public PromoCodeInstanceRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _promocodeinstances = dbContext.Set<PromoCodeInstance>();

        }
        public List<PromoCodeInstancesViewModel> GetAllReport(int pageNumber, int pageSize, out int count, int? promocodeId = null, bool? isValid = null, string promoCodeName = null, string studentName = null)
        {
            var promocodesInstancesquery = _promocodeinstances
                .Include(x => x.Student)
                .Include(x => x.PromoCode).AsQueryable();
            //.Where(x => (promocodeId != null ? x.PromoCodeId == promocodeId:true) &&
            //            (!string.IsNullOrEmpty(promoCodeName)? x.PromoCode.Name == promoCodeName:true) &&
            //            (!string.IsNullOrEmpty(studentName)?(x.Student.FirstName ==studentName || x.Student.LastName == studentName):true) && 
            //            (isValid == true? (x.IsUsed == false && (x.EndDate !=null?x.EndDate >= DateTime.Now:true)):true ) && )
            if (promocodeId != null)
            {
                promocodesInstancesquery = promocodesInstancesquery.Where(x => x.PromoCodeId == promocodeId);
            }
            if (!string.IsNullOrEmpty(promoCodeName))
            {
                promocodesInstancesquery = promocodesInstancesquery.Where(x => x.PromoCode.Name == promoCodeName);
            }
            if (!string.IsNullOrEmpty(studentName))
            {
                promocodesInstancesquery = promocodesInstancesquery.Where(x => x.Student.FirstName == studentName || x.Student.LastName == studentName);
            }
            if (isValid != null && isValid == true)
            {
                promocodesInstancesquery = promocodesInstancesquery.Where(x => (x.IsUsed == false && (x.EndDate != null ? x.EndDate >= DateTime.Now : true)));
            }
            if (isValid != null && isValid == false)
            {
                promocodesInstancesquery = promocodesInstancesquery.Where(x => ((x.IsUsed == true && x.EndDate == null) || x.EndDate < DateTime.Now));
            }
            count = promocodesInstancesquery.Count();
            var promocodesInstancesList = promocodesInstancesquery.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => new PromoCodeInstancesViewModel()
            {
                PromoCodeId = x.PromoCodeId,
                PromoCodeName = x.PromoCode.Name,
                PromoCodeKey = x.PromoCodeKey,
                Id = x.Id,
                IsUsed = x.IsUsed,
                StudentId = x.StudentId,
                StudentName = $"{x.Student.FirstName} {x.Student.LastName}",
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                CreatedDate = x.CreatedDate,
                StudentEmail = x.StudentEmail,
                PromoCodeValue = x.PromoCode.Value
            }).ToList();
            return promocodesInstancesList;
        }

        public PromoCodeInstancesViewModel GetByPromoCodeKey(string promoKey)
        {
            return _promocodeinstances
                .Include(x => x.PromoCode)
                .Include(x => x.Student)
                .Where(x => x.PromoCodeKey == promoKey).DefaultIfEmpty().Select(x => new PromoCodeInstancesViewModel()
                {
                    Id = x.Id,
                    PromoCodeId = x.PromoCodeId,
                    PromoCodeName = x.PromoCode.Name,
                    PromoCodeKey = x.PromoCodeKey,
                    StudentId = x.StudentId,
                    StudentName = $"{x.Student.FirstName} {x.Student.LastName}",
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsUsed = x.IsUsed,
                    CreatedDate = x.CreatedDate,
                    StudentEmail = x.StudentEmail,
                    PromoCodeValue = x.PromoCode.Value
                }).FirstOrDefault();
        }
        public PromoCodeInstance GetById(int id)
        {
            return _promocodeinstances
                .Include(x => x.PromoCode)
                .Include(x => x.Student)
                .Where(x => x.Id == id).DefaultIfEmpty().FirstOrDefault();
        }
    }

}
