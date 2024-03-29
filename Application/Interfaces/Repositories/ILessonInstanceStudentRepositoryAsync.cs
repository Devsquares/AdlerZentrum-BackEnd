﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface ILessonInstanceStudentRepositoryAsync : IGenericRepositoryAsync<LessonInstanceStudent>
    {
        IEnumerable<LessonInstanceStudent> GetStudentsByLessonInstance(int LessonInstanceId);
    }
}
