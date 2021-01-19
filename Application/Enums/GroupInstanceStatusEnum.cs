using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Enums
{
    public enum GroupInstanceStatusEnum
    {
        Pending = 0,
        Running = 1,
        Finished = 2,
        Canceld = 3,
        SlotCompleted = 4 // number of slotes completed
    }
}
