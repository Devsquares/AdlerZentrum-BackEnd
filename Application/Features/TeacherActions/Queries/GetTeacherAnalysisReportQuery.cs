using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetTeacherAnalysisReportQuery : IRequest<PagedResponse<IEnumerable<TeacherAnalysisReportModel>>>
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string teacherName { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public int? homeworksUploadDelayFrom { get; set; }
        public int? homeworksUploadDelayTo { get; set; }
        public int? homeworksCorrectionDelayFrom { get; set; }
        public int? homeworksCorrectionDelayTo { get; set; }
        public int? testsCorrectionDelayFrom { get; set; }
        public int? testsCorrectionDelayTo { get; set; }
        public int? feedbackScoreFrom { get; set; }
        public int? feedbackScoreto { get; set; }

        public class GetTeacherAnalysisReportQueryHandler : IRequestHandler<GetTeacherAnalysisReportQuery, PagedResponse<IEnumerable<TeacherAnalysisReportModel>>>
        {
            private readonly IHomeWorkSubmitionRepositoryAsync _homeWorkSubmitionRepositoryAsync;
            private readonly ILessonInstanceRepositoryAsync _lessonInstanceRepositoryAsync;
            private readonly ITestInstanceRepositoryAsync _testInstanceRepositoryAsync;
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroupInstanceAssignment;
            public GetTeacherAnalysisReportQueryHandler(IHomeWorkSubmitionRepositoryAsync homeWorkSubmitionRepositoryAsync,
                ILessonInstanceRepositoryAsync lessonInstanceRepositoryAsync,
                ITestInstanceRepositoryAsync testInstanceRepositoryAsync,
                ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroupInstanceAssignment)
            {
                _homeWorkSubmitionRepositoryAsync = homeWorkSubmitionRepositoryAsync;
                _lessonInstanceRepositoryAsync = lessonInstanceRepositoryAsync;
                _testInstanceRepositoryAsync = testInstanceRepositoryAsync;
                _teacherGroupInstanceAssignment = teacherGroupInstanceAssignment;
            }
            public async Task<PagedResponse<IEnumerable<TeacherAnalysisReportModel>>> Handle(GetTeacherAnalysisReportQuery command, CancellationToken cancellationToken)
            {
                if (command.pageNumber == 0) command.pageNumber = 1;
                if (command.pageSize == 0) command.pageSize = 10;
                int count = 0;
                var teachers = _teacherGroupInstanceAssignment.GetTeacherAnalysisReport(command.pageNumber, command.pageSize, command.teacherName, command.from, command.to, command.homeworksUploadDelayFrom, command.homeworksUploadDelayTo, command.homeworksCorrectionDelayFrom, command.homeworksCorrectionDelayTo, command.testsCorrectionDelayFrom, command.testsCorrectionDelayTo, command.feedbackScoreFrom,
                    command.feedbackScoreto, out count);

                return new PagedResponse<IEnumerable<TeacherAnalysisReportModel>>(teachers, command.pageNumber, command.pageSize, count);
            }
        }
    }
}
