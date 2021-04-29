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

namespace Application.DTOs
{
    public class StudentAnalysisReportQuery : IRequest<PagedResponse<IEnumerable<StudentAnalysisReportModel>>>
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string studentName { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public int? attendancefrom { get; set; }
        public int? attendanceto { get; set; }
        public int? LateSubmissionsfrom { get; set; }
        public int? LateSubmissionsto { get; set; }
        public int? MissedSubmissionsfrom { get; set; }
        public int? MissedSubmissionsto { get; set; }
        public int? CurrentProgressPointsfrom { get; set; }
        public int? CurrentProgressPointsTo { get; set; }

        public class StudentAnalysisReportQueryHandler : IRequestHandler<StudentAnalysisReportQuery, PagedResponse<IEnumerable<StudentAnalysisReportModel>>>
        {
            private readonly IAccountService _accountService;
            public StudentAnalysisReportQueryHandler(IAccountService accountService)
            {
                _accountService = accountService;
            }
            public async Task<PagedResponse<IEnumerable<StudentAnalysisReportModel>>> Handle(StudentAnalysisReportQuery command, CancellationToken cancellationToken)
            {
                if (command.pageNumber == 0) command.pageNumber = 1;
                if (command.pageSize == 0) command.pageSize = 10;
                int count = 0;
                var teachers = _accountService.GetStudentAnalysisReport(command.pageNumber, command.pageSize, command.studentName, command.from, command.to, command.attendancefrom, command.attendanceto, command.LateSubmissionsfrom, command.LateSubmissionsto, command.MissedSubmissionsfrom, command.MissedSubmissionsto, command.CurrentProgressPointsfrom,
                    command.CurrentProgressPointsTo, out count);

                return new PagedResponse<IEnumerable<StudentAnalysisReportModel>>(teachers, command.pageNumber, command.pageSize, count);
            }
        }
    }
}
