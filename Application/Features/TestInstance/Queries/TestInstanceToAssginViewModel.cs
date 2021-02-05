using Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class TestInstanceToAssginViewModel
    {
        public string StudentName { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string GroupSerial { get; set; }
        public string TestName { get; set; }
        public TestTypeEnum TestType { get; set; }
        public int Status { get; set; }
    }
}
