using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface ITeacherGroupInstanceAssignmentRepositoryAsync : IGenericRepositoryAsync<TeacherGroupInstanceAssignment>
    {
        IEnumerable<TeacherGroupInstanceAssignment> GetByTeacher(string TeacherId);
        TeacherGroupInstanceAssignment GetByGroupInstanceId(int groupInstanceId);
        TeacherGroupInstanceAssignment GetByTeachGroupInstanceId(string TeacherId, int groupInstanceId);
        List<TeacherGroupInstanceAssignment> GetListByGroupInstanceId(int groupInstanceId);
        List<TeacherGroupInstanceAssignment> GetAll(int pageNumber, int pageSize, out int totalCount, int? sublevelId = null, int? groupDefinationId = null);
        TeacherGroupInstanceAssignment GetFirstNotIsDefault(int groupInstanceId);
        TeacherGroupInstanceAssignment GetByTeacherId(string TeacherId);
        public TeacherGroupInstanceAssignment GetByGroupInstanceIdWithoutDefault(int groupInstanceId);
        List<TeacherAnalysisReportModel> GetTeacherAnalysisReport(int pageNumber, int PageSize, string teacherName, DateTime? from, DateTime? to
            , int? homeworksUploadDelayFrom, int? homeworksUploadDelayTo, int? homeworksCorrectionDelayFrom, int? homeworksCorrectionDelayTo,
            int? testsCorrectionDelayFrom, int? testsCorrectionDelayTo, int? feedbackScoreFrom, int? feedbackScoreto, out int count);
    }
}
