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
    public partial class RemoveDuplicateExceptionRequestCommand : IRequest<Response<bool>>
    {
        public string Email { get; set; }
    }

    public class RemoveDuplicateExceptionRequestCommandHandler : IRequestHandler<RemoveDuplicateExceptionRequestCommand, Response<bool>>
    {
        private readonly IDuplicateExceptionRepositoryAsync _duplicateExceptionRepository;
        private readonly IMapper _mapper;
        public RemoveDuplicateExceptionRequestCommandHandler(IDuplicateExceptionRepositoryAsync duplicateExceptionRepositoryAsync, IMapper mapper)
        {
            _duplicateExceptionRepository = duplicateExceptionRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<bool>> Handle(RemoveDuplicateExceptionRequestCommand request, CancellationToken cancellationToken)
        {

            var res = _duplicateExceptionRepository.GetByEmail(request.Email);
            await _duplicateExceptionRepository.DeleteAsync(res);
            return new Response<bool>(true);
        }
    }
}
