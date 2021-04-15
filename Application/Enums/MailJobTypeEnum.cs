using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Enums
{
    public enum MailJobTypeEnum
    {
        Registeration,
        GroupActivationStudent,
        GroupActivationTeacher,
        Banning,
        Disqualification,
        DownGrading,
        SendMessageToAdmin,
        SendMessageToInstructor,
        HomeworkAssignment,
        TestCorrected,
        HomeworkCorrected,
        HomeworkSubmitted,
        TestSubmitted,
        ContactUs,
        AcceptTeacherAbsence,
        RejectTeacherAbsence,
        AcceptTeacherAbsenceWithAnotherTeacher,
        RequestAbsenceToSuperVisor
    }
}
