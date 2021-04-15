using Application.Enums;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public partial class CreateDuplicateExceptionRequestCommand : IRequest<Response<int>>
    {
        public string Email { get; set; }
    }

    public class CreateDuplicateExceptionRequestCommandHandler : IRequestHandler<CreateDuplicateExceptionRequestCommand, Response<int>>
    {
        private readonly IDuplicateExceptionRepositoryAsync _duplicateExceptionRepository;
        private readonly IMapper _mapper;
        public CreateDuplicateExceptionRequestCommandHandler(IDuplicateExceptionRepositoryAsync duplicateExceptionRepositoryAsync, IMapper mapper)
        {
            _duplicateExceptionRepository = duplicateExceptionRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateDuplicateExceptionRequestCommand request, CancellationToken cancellationToken)
        {
            var obj = new DuplicateException()
            {
                Email = request.Email
            };

            var res = await _duplicateExceptionRepository.AddAsync(obj);
            return new Response<int>(res.Id);
        }
    }
}
