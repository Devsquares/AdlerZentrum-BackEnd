using Application.Enums;
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
    public class CreateGroupDefinitionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int SubLevelId { get; set; }
        public int TimeSlotId { get; set; }
        public int PricingId { get; set; }
        public int GroupConditionId { get; set; }
        public double Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? FinalTestDate { get; set; }
        public int MaxInstances { get; set; }

        public class CreateGroupDefinitionCommandHandler : IRequestHandler<CreateGroupDefinitionCommand, Response<int>>
        {
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepository;
            private readonly ISublevelRepositoryAsync _subLevelRepository;
            public CreateGroupDefinitionCommandHandler(IGroupDefinitionRepositoryAsync GroupDefinitionRepository, ISublevelRepositoryAsync sublevelRepositoryAsync)
            {
                _GroupDefinitionRepository = GroupDefinitionRepository;
                _subLevelRepository = sublevelRepositoryAsync;
            }
            public async Task<Response<int>> Handle(CreateGroupDefinitionCommand command, CancellationToken cancellationToken)
            {
                var groupDefinition = new Domain.Entities.GroupDefinition();

                Reflection.CopyProperties(command, groupDefinition);
                await checkTimeSlots(groupDefinition);
                groupDefinition.Status = (int)GroupDefinationStatusEnum.New;
                await _GroupDefinitionRepository.AddAsync(groupDefinition);
                return new Response<int>(groupDefinition.Id);

            }

            async Task checkTimeSlots(Domain.Entities.GroupDefinition groupDefinition)
            {
                Dictionary<int, int> lessonDates = new Dictionary<int, int>();
                DateTime iterationDay; int iterationDayWeekDay;

                int LessonOrder = 0;

                //Get the count of lessons
                Sublevel sublevel = await _subLevelRepository.GetByIdAsync(groupDefinition.SubLevelId);
                int noOfLessons = sublevel.NumberOflessons;

                //Get the start,end date of the group 
                DateTime startDate = groupDefinition.StartDate;
                DateTime endDate = groupDefinition.EndDate;
                if (startDate >= endDate) throw new ApiException("Start date can't be after the end date.");


                for (iterationDay = startDate; iterationDay <= endDate; iterationDay = iterationDay.AddDays(1))
                {
                    iterationDayWeekDay = (int)iterationDay.DayOfWeek + 1; //add one to use the same calender of the front
                    if (iterationDayWeekDay == 7) iterationDayWeekDay = 0; //Saturday
                    LessonOrder++;
                    //Add the lesson dates to the dictionary
                    lessonDates.Add(LessonOrder, 1);
                    //If no. of lessons is reached => stop.
                    if (lessonDates.Count == noOfLessons) break;
                }

                if (lessonDates.Count < noOfLessons) throw new ApiException("The system can't find " + noOfLessons + " time slots between the start and the end date.");
            }
        }
    }
}
