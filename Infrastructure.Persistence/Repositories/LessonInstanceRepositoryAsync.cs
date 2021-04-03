﻿using Application.Exceptions;
using Application.Features;
using Application.Helpers;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class LessonInstanceRepositoryAsync : GenericRepositoryAsync<LessonInstance>, ILessonInstanceRepositoryAsync
    {
        private readonly DbSet<LessonInstance> lessonInstances;
        private readonly DbSet<TimeSlotDetails> timeSlotDetails;
        public LessonInstanceRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            lessonInstances = dbContext.Set<LessonInstance>();
            timeSlotDetails = dbContext.Set<TimeSlotDetails>();
        }
        public IEnumerable<LessonInstance> GetByGroupInstanceId(int GroupInstanceId)
        {
            return lessonInstances
                .Include(x => x.GroupInstance)
                .Where(x => x.GroupInstanceId == GroupInstanceId).OrderBy(x => x.LessonDefinitionId).ToList();
        }
        public async override Task<LessonInstance> GetByIdAsync(int id)
        {
            return await lessonInstances
            .Include(x => x.LessonDefinition.Sublevel)
            .Include(x => x.LessonInstanceStudents)
            .ThenInclude(x => x.Student).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Dictionary<int, TimeFrame>> GetTimeSlotInstancesSorted(GroupInstance groupInstance)
        {
            Dictionary<int, TimeFrame> lessonDates = new Dictionary<int, TimeFrame>();
            DateTime LessonStartDate, LessonEndDate;

            DateTime iterationDay; int iterationDayWeekDay;

            TimeSlotDetails singleTimeSlotDetails;
            DateTime singleTimeSlotDetailsStart;
            DateTime singleTimeSlotDetailsEnd;

            int LessonOrder = 0;

            //Get the count of lessons
            int noOfLessons = groupInstance.GroupDefinition.Sublevel.NumberOflessons;

            //Get the list and the ccount of time slot details
            int TimeSlotId = groupInstance.GroupDefinition.TimeSlotId;
            var timeSlotDetailsList = timeSlotDetails.Where(x => x.TimeSlotId == TimeSlotId);

            //Get the start,end date of the group 
            DateTime startDate = groupInstance.GroupDefinition.StartDate;
            DateTime endDate = groupInstance.GroupDefinition.EndDate;
            if (startDate >= endDate) throw new ApiException("Start date can't be after the end date.");


            for (iterationDay = startDate; iterationDay <= endDate; iterationDay = iterationDay.AddDays(1))
            {
                iterationDayWeekDay = (int)iterationDay.DayOfWeek + 1; //add one to use the same calender of the front
                if (iterationDayWeekDay == 7) iterationDayWeekDay = 0; //Saturday

                //if it is not a time slot, continue
                if (!timeSlotDetailsList.Select(x => x.WeekDay).Contains(iterationDayWeekDay)) continue;

                LessonOrder++;

                //find the suitable time slot
                singleTimeSlotDetails = await timeSlotDetails.Where(x => x.WeekDay == iterationDayWeekDay).FirstOrDefaultAsync();

                //find the start and the end of the suitable time slot
                singleTimeSlotDetailsStart = singleTimeSlotDetails.TimeFrom;
                singleTimeSlotDetailsEnd = singleTimeSlotDetails.TimeTo;

                //calculate the lesson start and end date
                LessonStartDate = new DateTime(iterationDay.Year, iterationDay.Month, iterationDay.Day,
                    singleTimeSlotDetailsStart.Hour, singleTimeSlotDetailsStart.Minute, singleTimeSlotDetailsStart.Second);
                LessonEndDate = new DateTime(iterationDay.Year, iterationDay.Month, iterationDay.Day,
                    singleTimeSlotDetailsEnd.Hour, singleTimeSlotDetailsEnd.Minute, singleTimeSlotDetailsEnd.Second);

                //Add the lesson dates to the dictionary
                lessonDates.Add(LessonOrder, new TimeFrame { Start = LessonStartDate, End = LessonEndDate });

                //If no. of lessons is reached => stop.
                if (lessonDates.Count == noOfLessons) break;
            }


            return lessonDates;
        }

        public async Task<List<LateSubmissionsViewModel>> GetLateSubmissions(string TeacherName)
        {
            return await lessonInstances.Where(x => x.SubmissionDate == null || x.SubmissionDate > x.EndDate.AddDays(1)
            && String.IsNullOrEmpty(TeacherName) ? true :
           (x.SubmittedReportTeacher.FirstName.Contains(TeacherName) || x.SubmittedReportTeacher.LastName.Contains(TeacherName))
            )
              .Select(x => new LateSubmissionsViewModel()
              {
                  Id = x.Id,
                  Teacher = x.SubmittedReportTeacher,
                  SubmissionDate = x.SubmissionDate.Value,
                  ExpectedDate = x.EndDate.AddDays(1),
                  DelayDuration = (x.SubmissionDate.Value - x.EndDate.AddDays(1)).Hours
              })
              .ToListAsync();
        }

        public int GetLateSubmissionsCount(string TeacherName)
        {
            return lessonInstances.Where(x => x.SubmissionDate == null || x.SubmissionDate > x.EndDate.AddDays(1)
            && String.IsNullOrEmpty(TeacherName) ? true :
           (x.SubmittedReportTeacher.FirstName.Contains(TeacherName) || x.SubmittedReportTeacher.LastName.Contains(TeacherName))).Count();
        }

    }
}
