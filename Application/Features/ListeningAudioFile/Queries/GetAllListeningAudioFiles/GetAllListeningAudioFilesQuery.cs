using Application.Filters;
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
    public class GetAllListeningAudioFilesQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllListeningAudioFilesViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Dictionary<string, string> FilterValue { get; set; }
        public Dictionary<string, string> FilterRange { get; set; }
        public Dictionary<string, List<string>> FilterArray { get; set; }
        public Dictionary<string, string> FilterSearch { get; set; }
        public string SortBy { get; set; }
        public string SortType { get; set; }
        public bool NoPaging { get; set; }
    }
    public class GetAllListeningAudioFilesQueryHandler : IRequestHandler<GetAllListeningAudioFilesQuery, FilteredPagedResponse<IEnumerable<GetAllListeningAudioFilesViewModel>>>
    {
        private readonly IListeningAudioFileRepositoryAsync _listeningaudiofileRepository;
        private readonly IMapper _mapper;
        public GetAllListeningAudioFilesQueryHandler(IListeningAudioFileRepositoryAsync listeningaudiofileRepository, IMapper mapper)
        {
            _listeningaudiofileRepository = listeningaudiofileRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllListeningAudioFilesViewModel>>> Handle(GetAllListeningAudioFilesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllListeningAudioFilesParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _listeningaudiofileRepository.GetCount(validFilter);

            var listeningaudiofile = await _listeningaudiofileRepository.GetPagedReponseAsync(validFilter);
            var listeningaudiofileViewModel = _mapper.Map<IEnumerable<GetAllListeningAudioFilesViewModel>>(listeningaudiofile);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllListeningAudioFilesViewModel>>(listeningaudiofileViewModel, validFilter, count);
        }
    }
}
