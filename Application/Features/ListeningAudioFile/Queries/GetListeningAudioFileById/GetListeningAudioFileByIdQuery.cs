using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetListeningAudioFileByIdQuery : IRequest<Response<Domain.Entities.ListeningAudioFile>>
    {
        public int Id { get; set; }
        public class GetListeningAudioFileByIdQueryHandler : IRequestHandler<GetListeningAudioFileByIdQuery, Response<Domain.Entities.ListeningAudioFile>>
        {
            private readonly IListeningAudioFileRepositoryAsync _listeningaudiofileRepository;
            public GetListeningAudioFileByIdQueryHandler(IListeningAudioFileRepositoryAsync listeningaudiofileRepository)
            {
                _listeningaudiofileRepository = listeningaudiofileRepository;
            }
            public async Task<Response<Domain.Entities.ListeningAudioFile>> Handle(GetListeningAudioFileByIdQuery query, CancellationToken cancellationToken)
            {
                var listeningaudiofile = await _listeningaudiofileRepository.GetByIdAsync(query.Id);
                if (listeningaudiofile == null) throw new ApiException($"ListeningAudioFile Not Found.");
                return new Response<Domain.Entities.ListeningAudioFile>(listeningaudiofile);
            }
        }
    }
}
