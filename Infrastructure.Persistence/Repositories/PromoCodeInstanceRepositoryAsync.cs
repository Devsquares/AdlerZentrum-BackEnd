using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using LinqKit;
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
                 .Include(x => x.GroupDefinition)
                .Include(x => x.PromoCode).AsQueryable(); 
            if (promocodeId != null)
            {
                promocodesInstancesquery = promocodesInstancesquery.Where(x => x.PromoCodeId == promocodeId);
            }
            if (!string.IsNullOrEmpty(promoCodeName))
            {
                promocodesInstancesquery = promocodesInstancesquery.Where(x => x.PromoCode.Name == promoCodeName);
            }
            if (!string.IsNullOrWhiteSpace(studentName))
            {
                var predicate = PredicateBuilder.New<PromoCodeInstance>();
                string[] searchWordsArr = studentName.Split(" ");
                foreach (var item in searchWordsArr)
                {
                    predicate.Or(x => x.Student.FirstName.ToLower().Contains(studentName.ToLower()) || x.Student.LastName.ToLower().Contains(item.ToLower()));
                }
                promocodesInstancesquery = promocodesInstancesquery.Where(predicate);
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
                PromoCodeValue = x.PromoCode.Value,
                GroupDefinitionSerial = x.GroupDefinition != null ? x.GroupDefinition.Serial : null
            }).ToList();
            return promocodesInstancesList;
        }

        public PromoCodeInstancesViewModel GetByPromoCodeKey(string promoKey,string studentId, string studentEmail)
        {
            var query =  _promocodeinstances
                .Include(x => x.PromoCode)
                .Include(x => x.Student)
                 .Include(x => x.GroupDefinition)
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
                    PromoCodeValue = x.PromoCode.Value,
                    GroupDefinitionSerial = x.GroupDefinition != null? x.GroupDefinition.Serial:null
                }).FirstOrDefault();

            if(string.IsNullOrEmpty(query.StudentId)&& string.IsNullOrEmpty(query.StudentEmail))
            {
                return query;
            }
            else if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(studentEmail) && query.StudentEmail.ToLower() != studentEmail.ToLower() && query.StudentId != studentId)
            {
                throw new Exception("This PromoCode Key not for this student");
            }
            else if(!string.IsNullOrEmpty(studentId) && query.StudentId!= studentId)
            {
                throw new Exception("This PromoCode Key not for this student");
            }
            else if (!string.IsNullOrEmpty(studentEmail) && query.StudentEmail.ToLower() != studentEmail.ToLower())
            {
                throw new Exception("This PromoCode Key not for this student");
            }
            return query;
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
