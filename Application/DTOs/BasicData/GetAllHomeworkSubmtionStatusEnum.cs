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
    public class GetAllHomeworkSubmtionStatusEnumQuery : IRequest<List<string>>
    {

    }
    public class GetAllHomeworkSubmtionStatusEnum : IRequestHandler<GetAllHomeworkSubmtionStatusEnumQuery, List<string>>
    {
        public async Task<List<string>> Handle(GetAllHomeworkSubmtionStatusEnumQuery request, CancellationToken cancellationToken)
        {
            return Enum.GetNames(typeof(HomeWorkSubmitionStatusEnum)).ToList();
        }
    }
}
