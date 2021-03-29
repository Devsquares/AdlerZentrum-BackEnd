using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Enums
{
    public enum MailJobTypeEnum
    {
        Registeration = 0,
        GroupActivation = 1,
        Banning = 2,
        Disqualification = 3,
        SendMessageToAdmin = 4,
        SendMessageToInstructor = 5,
        HomeworkAssignment = 6,
        TestCorrected = 7,
        HomeworkCorrected = 8,
        HomeworkSubmitted = 9,
        TestSubmitted = 10
    }
}
