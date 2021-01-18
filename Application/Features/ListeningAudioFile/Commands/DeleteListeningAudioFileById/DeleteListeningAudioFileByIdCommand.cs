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
    public class DeleteListeningAudioFileByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteListeningAudioFileByIdCommandHandler : IRequestHandler<DeleteListeningAudioFileByIdCommand, Response<int>>
        {
            private readonly IListeningAudioFileRepositoryAsync _listeningaudiofileRepository;
            public DeleteListeningAudioFileByIdCommandHandler(IListeningAudioFileRepositoryAsync listeningaudiofileRepository)
            {
                _listeningaudiofileRepository = listeningaudiofileRepository;
            }
            public async Task<Response<int>> Handle(DeleteListeningAudioFileByIdCommand command, CancellationToken cancellationToken)
            {
                var listeningaudiofile = await _listeningaudiofileRepository.GetByIdAsync(command.Id);
                if (listeningaudiofile == null) throw new ApiException($"ListeningAudioFile Not Found.");
                await _listeningaudiofileRepository.DeleteAsync(listeningaudiofile);
                return new Response<int>(listeningaudiofile.Id);
            }
        }
    }
}
