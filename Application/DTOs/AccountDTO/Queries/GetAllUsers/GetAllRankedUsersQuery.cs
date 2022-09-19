using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllRankedUsersQuery : IRequest<List<KeyValuePair<string, RankModel>>>
    {
        public string UserId { get; set; }
        public bool isInstance { get; set; }
    }
    public class GetAllRankedUsersQueryHandler : IRequestHandler<GetAllRankedUsersQuery, List<KeyValuePair<string, RankModel>>>
    {
        private readonly IAccountService _userRepository;
        private readonly IGroupInstanceStudentRepositoryAsync _GroupInstanceStudentRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllRankedUsersQueryHandler(IAccountService userRepository, IMapper mapper,
              IGroupInstanceStudentRepositoryAsync GroupInstanceStudentRepositoryAsync)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _GroupInstanceStudentRepositoryAsync = GroupInstanceStudentRepositoryAsync;
        }

        public async Task<List<KeyValuePair<string, RankModel>>> Handle(GetAllRankedUsersQuery request, CancellationToken cancellationToken)
        {

            Random rd = new Random();
            List<KeyValuePair<string, RankModel>> rankedStudents = new List<KeyValuePair<string, RankModel>>();
            // KeyValuePair<string, RankModel> rankedStudent = new KeyValuePair<string, RankModel>();
            RankModel rankmodel = new RankModel();
            List<GroupInstanceStudents> users;
            if (request.isInstance)
            {
                users = await _GroupInstanceStudentRepositoryAsync.GetAllStudentInGroupInstanceByStudentId(request.UserId);

            }
            else
            {
                users = await _GroupInstanceStudentRepositoryAsync.GetAllStudentInGroupDefinitionByStudentId(request.UserId);
            }
            //foreach (var item in users)
            //{
            //    int rand_num = rd.Next(1, 20);
            //    rankedStudents.Add(new KeyValuePair<string, RankModel>(item.Id, new RankModel() { StudentName = item.FirstName, Rank = rand_num }));
            //}
            int rank = 0;
            for (int i = 1; i <= users.Count; i++)
            {
                rank = i;
                if (i > 1 && users[i - 1].AchievedScore == users[i - 2].AchievedScore)
                {
                    rank = i - 2;
                }
                rankedStudents.Add(new KeyValuePair<string, RankModel>(users[i - 1].StudentId, new RankModel() { StudentName = users[i - 1].Student.FirstName + " " + users[i - 1].Student.LastName, Rank = rank }));
            }
            rankedStudents = rankedStudents.OrderBy(x => x.Value.Rank).ToList();
            return new List<KeyValuePair<string, RankModel>>(rankedStudents);
        }
    }
}
