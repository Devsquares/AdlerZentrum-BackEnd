using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class ForumTopicRepositoryAsync : GenericRepositoryAsync<ForumTopic>, IForumTopicRepositoryAsync
    {
        private readonly DbSet<ForumTopic> _forumTopics;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;


        public ForumTopicRepositoryAsync(ApplicationDbContext dbContext,
            IGroupInstanceRepositoryAsync groupInstanceRepositoryAsync,
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager) : base(dbContext)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _forumTopics = _dbContext.Set<ForumTopic>();
            _groupInstanceRepositoryAsync = groupInstanceRepositoryAsync;

        }

        public new async Task<ForumTopic> GetByIdAsync(int id)
        {
            return await _forumTopics.Include(b => b.GroupInstance).Include(b => b.GroupDefinition).Include(b => b.Writer).
                Where(b=>b.Id == id).FirstOrDefaultAsync();

        }
        public async Task<IReadOnlyList<ForumTopic>> GetPagedReponseAsync(int pageNumber, int pageSize, string userId, ForumType forumType, int groupInstanceId, int groupDefinitionId)
        {
            bool student;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ApiException("Mo user found with the id " + userId);
            if (await _userManager.IsInRoleAsync(user, Application.Enums.RolesEnum.Student.ToString()))
                student = true;
            else
                student = false;


            //wrong info
            if ((int)forumType == 0)
                throw new ApiException("Forum type must be specified");
            if (!Enum.GetValues(typeof(ForumType)).Cast<ForumType>().Contains(forumType))
                throw new ApiException("Forum type is unknown.");

            //student
            if (student)
            {
                //wrong info
                if (groupInstanceId != 0 || groupDefinitionId != 0)
                    throw new ApiException("Topics for students can't be searched for a specific group definition or instance.");

                //group instances & group definitions
                if (forumType == ForumType.GroupInstance || forumType == ForumType.GroupDefinition)
                {
                    var activeGroupInstance = _groupInstanceRepositoryAsync.GetActiveGroupInstance(userId);
                    if (activeGroupInstance == 0 || activeGroupInstance == null)
                        throw new ApiException("Student with id " + userId + " is not assigned to a group instance");

                    // group instance
                    if (forumType == ForumType.GroupInstance)
                    {
                        groupInstanceId = (int)_groupInstanceRepositoryAsync.GetActiveGroupInstance(userId);
                    } else
                    // group definition
                    {
                        var groupInstance = await _dbContext.Set<GroupInstance>().FindAsync(activeGroupInstance);
                        var groupDefinition = await _dbContext.Set<GroupDefinition>().FindAsync(groupInstance.GroupDefinitionId);
                        groupDefinitionId = (int)groupDefinition.Id;
                    }
                }

                // staff
                else if (forumType == ForumType.Staff)
                {
                    throw new ApiException("Forum type Staff is not allowed for students.");
                }
            } else
            //staff
            {
                //group instance
                if (forumType == ForumType.GroupInstance && groupInstanceId == 0)
                {
                    throw new ApiException("Topics for forum type " + ((Application.Enums.ForumType)forumType).ToString() + " must be  searched for a specific groupInstance.");
                }

                //group definition
                else if (forumType == ForumType.GroupDefinition && groupDefinitionId == 0)
                {
                    throw new ApiException("Topics for forum type " + ((Application.Enums.ForumType)forumType).ToString() + " must be searched for a specific groupDefinition.");
                }

                //staff or organization
                else if ((forumType == ForumType.Staff || forumType == ForumType.Organization)
                    && (groupInstanceId != 0 || groupDefinitionId != 0))
                {
                    throw new ApiException("Topics for forum type " + ((Application.Enums.ForumType)forumType).ToString() + " can't be searched for a specific group definition or instance.");
                }
            }


            return await _dbContext.Set<ForumTopic>()
                //.Include(b=> b.GroupInstance).Include(b => b.GroupDefinition)
                .Include(b => b.Writer)
                .Where(b => (forumType == 0 || b.ForumType == (int)forumType) && (groupInstanceId == 0 || b.GroupInstanceId == groupInstanceId) && (groupDefinitionId == 0 || b.GroupDefinitionId == groupDefinitionId))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public int GetCount(string userId, ForumType forumType, int groupInstanceId, int groupDefinitionId)
        {
            bool student;
            var user =  _userManager.FindByIdAsync(userId).Result;
            if (user == null)
                throw new ApiException("Mo user found with the id " + userId);
            if ( _userManager.IsInRoleAsync(user, Application.Enums.RolesEnum.Student.ToString()).Result)
                student = true;
            else
                student = false;


            //wrong info
            if ((int)forumType == 0)
                throw new ApiException("Forum type must be specified");
            if (!Enum.GetValues(typeof(ForumType)).Cast<ForumType>().Contains(forumType))
                throw new ApiException("Forum type is unknown.");

            //student
            if (student)
            {
                //wrong info
                if (groupInstanceId != 0 || groupDefinitionId != 0)
                    throw new ApiException("Topics for students can't be searched for a specific group definition or instance.");

                //group instances & group definitions
                if (forumType == ForumType.GroupInstance || forumType == ForumType.GroupDefinition)
                {
                    var activeGroupInstance = _groupInstanceRepositoryAsync.GetActiveGroupInstance(userId);
                    if (activeGroupInstance == 0 || activeGroupInstance == null)
                        throw new ApiException("Student with id " + userId + " is not assigned to a group instance");

                    // group instance
                    if (forumType == ForumType.GroupInstance)
                    {
                        groupInstanceId = (int)_groupInstanceRepositoryAsync.GetActiveGroupInstance(userId);
                    }
                    else
                    // group definition
                    {
                        var groupInstance = _dbContext.Set<GroupInstance>().FindAsync(activeGroupInstance).Result;
                        var groupDefinition = _dbContext.Set<GroupDefinition>().FindAsync(groupInstance.GroupDefinitionId).Result;
                        groupDefinitionId = (int)groupDefinition.Id;
                    }
                }

                // staff
                else if (forumType == ForumType.Staff)
                {
                    throw new ApiException("Forum type Staff is not allowed for students.");
                }
            }
            else
            //staff
            {
                //group instance
                if (forumType == ForumType.GroupInstance && groupInstanceId == 0)
                {
                    throw new ApiException("Topics for forum type " + ((Application.Enums.ForumType)forumType).ToString() + " must be  searched for a specific groupInstance.");
                }

                //group definition
                else if (forumType == ForumType.GroupDefinition && groupDefinitionId == 0)
                {
                    throw new ApiException("Topics for forum type " + ((Application.Enums.ForumType)forumType).ToString() + " must be searched for a specific groupDefinition.");
                }

                //staff or organization
                else if ((forumType == ForumType.Staff || forumType == ForumType.Organization)
                    && (groupInstanceId != 0 || groupDefinitionId != 0))
                {
                    throw new ApiException("Topics for forum type " + ((Application.Enums.ForumType)forumType).ToString() + " can't be searched for a specific group definition or instance.");
                }
            }
            return _dbContext.Set<ForumTopic>()
                .Where(b => (forumType == 0 || b.ForumType == (int)forumType) && (groupInstanceId == 0 || b.GroupInstanceId == groupInstanceId) && (groupDefinitionId == 0 || b.GroupDefinitionId == groupDefinitionId))
                .Count();
        }

        public new async Task<ForumTopic> AddAsync(ForumTopic entity)
        {
            entity = await ValidateCreation(entity);

            return await base.AddAsync(entity);
        }

        private async Task<ForumTopic> ValidateCreation(ForumTopic entity)
        {
            bool student;
            var userId = entity.WriterId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ApiException("Mo user found with the id " + entity.WriterId);
            if (await _userManager.IsInRoleAsync(user, Application.Enums.RolesEnum.Student.ToString()))
                student = true;
            else
                student = false;

            var forumType = (ForumType)entity.ForumType;
            if (!Enum.GetValues(typeof(ForumType)).Cast<ForumType>().Contains(forumType))
                throw new ApiException("Forum type is unknown.");

            if (student)
                return await ValidateCreationForStudent(entity);
            else
                return await ValidateCreationForStaff(entity);

        }

        private async Task<ForumTopic> ValidateCreationForStudent(ForumTopic entity)
        {
            var forumType = entity.ForumType;

            //wrong info
            if (forumType == 0)
                throw new ApiException("Forum type must be specified");

            //wrong info
            if (entity.GroupInstanceId != 0 || entity.GroupDefinitionId != 0)
                throw new ApiException("Topics for students can't be assigned to a specific group definition or instance.");

            //staff
            if (forumType == (int)ForumType.Staff)
                throw new ApiException("Forum type Staff is not allowed for students.");

            //organization
            if (forumType == (int)ForumType.Organization)
                return entity;

            //group instance or definition
            var activeGroupInstance = _groupInstanceRepositoryAsync.GetActiveGroupInstance(entity.WriterId);
            if (activeGroupInstance == 0 || activeGroupInstance == null)
                throw new ApiException("Student with id " + entity.WriterId + " is not assigned to a group instance");

            //group instance
            if (forumType == (int)ForumType.GroupInstance)
            {
                entity.GroupInstanceId = (int)activeGroupInstance;
                return entity;
            }

            //group definition
            var groupInstance = await _dbContext.Set<GroupInstance>().FindAsync(activeGroupInstance);
            var groupDefinition = await _dbContext.Set<GroupDefinition>().FindAsync(groupInstance.GroupDefinitionId);
            entity.GroupDefinitionId = (int)groupDefinition.Id;
            return entity;
        }

        private async Task<ForumTopic> ValidateCreationForStaff(ForumTopic entity)
        {
            var forumType = entity.ForumType;

            //wrong info
            if (forumType == 0)
                throw new ApiException("Forum type must be specified");

            //group instance
            if (forumType == (int)ForumType.GroupInstance && entity.GroupInstanceId == 0)
            {
                throw new ApiException("Topics for forum type " + ((Application.Enums.ForumType)forumType).ToString() + " must be assigned to a specific groupInstance.");
            }

            //group definition
            else if (forumType == (int)ForumType.GroupDefinition && entity.GroupDefinitionId == 0)
            {
                throw new ApiException("Topics for forum type " + ((Application.Enums.ForumType)forumType).ToString() + " must be assigned to a specific groupDefinition.");
            }

            //staff or organization
            else if ((forumType == (int)ForumType.Staff || forumType == (int)ForumType.Organization)
                && (entity.GroupInstanceId != 0 || entity.GroupDefinitionId != 0))
            {
                throw new ApiException("Topics for forum type " + ((Application.Enums.ForumType)forumType).ToString() + " can't be assigned to a specific group definition or instance.");
            }

            return entity;
        }
    }
}
