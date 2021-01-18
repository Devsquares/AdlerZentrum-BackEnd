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
    public partial class CreateListeningAudioFileCommand : IRequest<Response<int>>
    {
		public string FileName { get; set; }
		public string FilePath { get; set; }
    }

    public class CreateListeningAudioFileCommandHandler : IRequestHandler<CreateListeningAudioFileCommand, Response<int>>
    {
        private readonly IListeningAudioFileRepositoryAsync _listeningaudiofileRepository;
        private readonly IMapper _mapper;
        public CreateListeningAudioFileCommandHandler(IListeningAudioFileRepositoryAsync listeningaudiofileRepository, IMapper mapper)
        {
            _listeningaudiofileRepository = listeningaudiofileRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateListeningAudioFileCommand request, CancellationToken cancellationToken)
        {
            var listeningaudiofile = _mapper.Map<Domain.Entities.ListeningAudioFile>(request);
            await _listeningaudiofileRepository.AddAsync(listeningaudiofile);
            return new Response<int>(listeningaudiofile.Id);
        }
    }
}
