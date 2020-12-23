using Application.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.BasicData
{
    public class GetSingleQuestionTypesQuery : IRequest<List<EnumsViewModel>>
    {
    }
    public class GetSingleQuestionTypesQueryHandler : IRequestHandler<GetSingleQuestionTypesQuery, List<EnumsViewModel>>
    {
        public async Task<List<EnumsViewModel>> Handle(GetSingleQuestionTypesQuery request, CancellationToken cancellationToken)
        {
            List<string> data = Enum.GetNames(typeof(SingleQuestionTypeEnum)).ToList();
            List<EnumsViewModel> outputs = new List<EnumsViewModel>();
            int index = 0;
            foreach (var item in data)
            {
                outputs.Add(new EnumsViewModel
                {
                    Id = index++,
                    Value = item
                }); 
            }
            return outputs;
        }
    }
}
