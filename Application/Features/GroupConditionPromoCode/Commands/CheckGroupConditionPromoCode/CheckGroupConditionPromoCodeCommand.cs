using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public partial class CheckGroupConditionPromoCodeCommand : IRequest<Response<bool>>
    {
		public int GroupInstanceId { get; set; }
		public int PromoCodeInstanceId { get; set; }
        public string StudentId { get; set; }
    }

    public class CheckGroupConditionPromoCodeCommandHandler : IRequestHandler<CheckGroupConditionPromoCodeCommand, Response<bool>>
    {
        private readonly IGroupConditionPromoCodeRepositoryAsync _groupconditionpromocodeRepository;
        private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepository;
        private readonly IMapper _mapper;
        public CheckGroupConditionPromoCodeCommandHandler(IGroupConditionPromoCodeRepositoryAsync groupconditionpromocodeRepository, IMapper mapper,
            IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepository)
        {
            _groupconditionpromocodeRepository = groupconditionpromocodeRepository;
            _mapper = mapper;
            _groupInstanceStudentRepository = groupInstanceStudentRepository;
        }

        public async Task<Response<bool>> Handle(CheckGroupConditionPromoCodeCommand request, CancellationToken cancellationToken)
        {
            var result = _groupconditionpromocodeRepository.CheckPromoCodeCountInGroupInstance(request.GroupInstanceId, request.PromoCodeInstanceId);
            if(result)
            {
                var studentValid = _groupInstanceStudentRepository.GetByStudentId(request.StudentId,request.GroupInstanceId);
                if (studentValid == null)
                {
                    GroupInstanceStudents groupInstanceStudents = new GroupInstanceStudents();
                    groupInstanceStudents.GroupInstanceId = request.GroupInstanceId;
                   groupInstanceStudents.PromoCodeInstanceId = request.PromoCodeInstanceId;
                    groupInstanceStudents.StudentId = request.StudentId;
                    await _groupInstanceStudentRepository.AddAsync(groupInstanceStudents);
                }
                else
                {
                    if(studentValid.GroupInstanceId == request.GroupInstanceId)
                    {
                        studentValid.PromoCodeInstanceId = request.PromoCodeInstanceId;
                        await _groupInstanceStudentRepository.UpdateAsync(studentValid);
                    }
                }
            }
            return new Response<bool>(result);
        }
    }
}
