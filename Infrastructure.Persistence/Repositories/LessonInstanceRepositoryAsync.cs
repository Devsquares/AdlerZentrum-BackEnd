using Application.Exceptions;
using Application.Features;
using Application.Helpers;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using LinqKit;
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
                // TODO: we can remove select.
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

            if (lessonDates.Count < noOfLessons) throw new ApiException("The system can't find " + noOfLessons + " time slots between the start and the end date.");

            return lessonDates;
        }

        public async Task<List<LateSubmissionsViewModel>> GetLateSubmissions(string TeacherName, int pageNumber, int pageSize, bool DelaySeen)
        {
            var query = lessonInstances.AsQueryable();
            if (!string.IsNullOrWhiteSpace(TeacherName))
            {
                var predicate = PredicateBuilder.New<LessonInstance>();
                string[] searchWordsArr = TeacherName.Split(" ");
                foreach (var item in searchWordsArr)
                {
                    predicate.Or(x => x.GroupInstance.TeacherAssignment.Where(x => x.IsDefault).FirstOrDefault().Teacher.FirstName.ToLower().Contains(TeacherName.ToLower()) || x.GroupInstance.TeacherAssignment.Where(x => x.IsDefault).FirstOrDefault().Teacher.LastName.ToLower().Contains(item.ToLower()));
                }
                query = query.Where(predicate);
            }

            return await query
                .Include(x => x.SubmittedReportTeacher)
                .Include(x => x.GroupInstance.TeacherAssignment)
                .Include(x => x.LessonDefinition)
                .Where(x => x.SubmissionDate == null || x.SubmissionDate > x.DueDate
            && x.DelaySeen == DelaySeen)
              .Select(x => new LateSubmissionsViewModel()
              {
                  Id = x.Id,
                  Teacher = x.SubmittedReportTeacher == null ? (x.GroupInstance.TeacherAssignment != null ? x.GroupInstance.TeacherAssignment.Where(x => x.IsDefault).FirstOrDefault().Teacher.FirstName.ToString() + " " + x.GroupInstance.TeacherAssignment.Where(x => x.IsDefault).FirstOrDefault().Teacher.LastName.ToString() : null)
                  : x.SubmittedReportTeacher.FirstName.ToString() + " " + x.SubmittedReportTeacher.LastName.ToString(),
                  SubmissionDate = x.SubmissionDate.Value,
                  ExpectedDate = x.DueDate.Value,
                  DelayDuration = (x.SubmissionDate.Value - x.DueDate.Value).Hours,
                  LessonInstance = x,
                  GroupInstance = x.GroupInstance
              }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(); 
        }

        public int GetLateSubmissionsCount(string TeacherName, bool DelaySeen)
        {
            return lessonInstances.Where(x => x.SubmissionDate == null || x.SubmissionDate > x.DueDate
            && x.DelaySeen == DelaySeen && String.IsNullOrEmpty(TeacherName) ? true :
           (x.SubmittedReportTeacher.FirstName + " " + x.SubmittedReportTeacher.LastName).Contains(TeacherName)).Count();
        }

    }
}
