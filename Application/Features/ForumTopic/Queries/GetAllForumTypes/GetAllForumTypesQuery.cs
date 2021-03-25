using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetAllForumTypesQuery : IRequest<Response<IEnumerable<GetAllForumTypesViewModel>>>
    {
    }
    public class GetAllForumTypesQueryHandler : IRequestHandler<GetAllForumTypesQuery, Response<IEnumerable<GetAllForumTypesViewModel>>>
    {
        private readonly IMapper _mapper;
        public GetAllForumTypesQueryHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<GetAllForumTypesViewModel>>> Handle(GetAllForumTypesQuery request, CancellationToken cancellationToken)
        {
            List<GetAllForumTypesViewModel> getAllForumTypesViewModels = new List<GetAllForumTypesViewModel>();
            var forumTypes = Enum.GetValues(typeof(Enums.ForumType)).Cast<Enums.ForumType>();
            foreach (Enums.ForumType forumType in forumTypes) 
                getAllForumTypesViewModels.Add(new GetAllForumTypesViewModel { ForumType = (int)forumType, Description = forumType.ToString() });

            return new Wrappers.Response<IEnumerable<GetAllForumTypesViewModel>>(getAllForumTypesViewModels);
        }

    }
}
