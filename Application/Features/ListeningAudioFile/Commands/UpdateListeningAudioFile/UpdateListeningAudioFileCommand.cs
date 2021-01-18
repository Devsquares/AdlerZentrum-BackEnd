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
    public class UpdateListeningAudioFileCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public class UpdateListeningAudioFileCommandHandler : IRequestHandler<UpdateListeningAudioFileCommand, Response<int>>
        {
            private readonly IListeningAudioFileRepositoryAsync _listeningaudiofileRepository;
            public UpdateListeningAudioFileCommandHandler(IListeningAudioFileRepositoryAsync listeningaudiofileRepository)
            {
                _listeningaudiofileRepository = listeningaudiofileRepository;
            }
            public async Task<Response<int>> Handle(UpdateListeningAudioFileCommand command, CancellationToken cancellationToken)
            {
                var listeningaudiofile = await _listeningaudiofileRepository.GetByIdAsync(command.Id);

                if (listeningaudiofile == null)
                {
                    throw new ApiException($"ListeningAudioFile Not Found.");
                }
                else
                {
                    listeningaudiofile.FileName = command.FileName;
                    listeningaudiofile.FilePath = command.FilePath;

                    await _listeningaudiofileRepository.UpdateAsync(listeningaudiofile);
                    return new Response<int>(listeningaudiofile.Id);
                }
            }
        }

    }
}
