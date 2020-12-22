using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class StudentsByLessonViewModel
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string Profilephoto { get; set; }
    }
}
