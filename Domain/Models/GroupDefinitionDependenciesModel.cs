using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class GroupDefinitionDependenciesModel
    {
        public Domain.Entities.GroupDefinition GroupDefinition { get; set; }
        public List<StudentsGroupInstanceModel> GroupInstancesStudents { get; set; }
        public List<StudentsModel> OverPaymentStudents { get; set; } = new List<StudentsModel>();
        public List<StudentsModel> InterestedStudents { get; set; } = new List<StudentsModel>();
    }
}
