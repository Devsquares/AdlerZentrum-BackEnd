using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public partial class CreateOverPaymentStudentCommand : IRequest<Response<int>>
    {
		public string StudentId { get; set; }
		public int GroupDefinitionId { get; set; }
    }

    public class CreateOverPaymentStudentCommandHandler : IRequestHandler<CreateOverPaymentStudentCommand, Response<int>>
    {
        private readonly IOverPaymentStudentRepositoryAsync _overpaymentstudentRepository;
        private readonly IMapper _mapper;
        public CreateOverPaymentStudentCommandHandler(IOverPaymentStudentRepositoryAsync overpaymentstudentRepository, IMapper mapper)
        {
            _overpaymentstudentRepository = overpaymentstudentRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateOverPaymentStudentCommand request, CancellationToken cancellationToken)
        {
            var overpaymentstudent = _mapper.Map<Domain.Entities.OverPaymentStudent>(request);
            await _overpaymentstudentRepository.AddAsync(overpaymentstudent);
            return new Response<int>(overpaymentstudent.Id);
        }
    }
}
