using Application.Enums;
using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.BasicData
{
    public class GetAllRolesQuery : IRequest<List<string>>
    {
    }
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<string>>
    {
        public async Task<List<string>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            return Enum.GetNames(typeof(RolesEnum)).ToList();
        }
    }
}
