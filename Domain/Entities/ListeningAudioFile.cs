using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ListeningAudioFile : AuditableBaseEntity
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
