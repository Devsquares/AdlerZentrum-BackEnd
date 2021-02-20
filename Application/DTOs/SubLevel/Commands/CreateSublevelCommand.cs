using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateSublevelCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LevelId { get; set; }
        public int NumberOflessons { get; set; }
        public string Color { get; set; }
        public bool IsFinal { get; set; }
        public int Quizpercent { get; set; }
        public int SublevelTestpercent { get; set; }
        public int FinalTestpercent { get; set; }

        public class CreateSublevelCommandHandler : IRequestHandler<CreateSublevelCommand, Response<int>>
        {
            private readonly ISublevelRepositoryAsync _SublevelRepository;
            public CreateSublevelCommandHandler(ISublevelRepositoryAsync SublevelRepository)
            {
                _SublevelRepository = SublevelRepository;
            }
            public async Task<Response<int>> Handle(CreateSublevelCommand command, CancellationToken cancellationToken)
            {
                var Sublevel = new Sublevel();

                Reflection.CopyProperties(command, Sublevel);
                for (int i = 0; i < command.NumberOflessons; i++)
                {
                    Sublevel.LessonDefinitions.Add(new LessonDefinition { Order = i + 1 });
                }
                await _SublevelRepository.AddAsync(Sublevel);
                return new Response<int>(Sublevel.Id);

            }
        }
    }
}
