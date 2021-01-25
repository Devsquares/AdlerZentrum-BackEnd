using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InterestedStudent.Commands.CreateInterestedStudent
{
    public partial class CreateInterestedStudentCommand : IRequest<Response<int>>
    {
		public int PromoCodeId { get; set; }
		public string StudentId { get; set; }
		public int GroupDefinitionId { get; set; }
    }

    public class CreateInterestedStudentCommandHandler : IRequestHandler<CreateInterestedStudentCommand, Response<int>>
    {
        private readonly IInterestedStudentRepositoryAsync _interestedstudentRepository;
        private readonly IMapper _mapper;
        public CreateInterestedStudentCommandHandler(IInterestedStudentRepositoryAsync interestedstudentRepository, IMapper mapper)
        {
            _interestedstudentRepository = interestedstudentRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateInterestedStudentCommand request, CancellationToken cancellationToken)
        {
            var interestedstudent = _mapper.Map<Domain.Entities.InterestedStudent>(request);
            await _interestedstudentRepository.AddAsync(interestedstudent);
            return new Response<int>(interestedstudent.Id);
        }
    }
}
