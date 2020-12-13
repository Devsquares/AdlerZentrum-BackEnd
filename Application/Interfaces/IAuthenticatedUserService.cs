﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
        string Role { get; }
        int? GroupInstanceId { get; }
    }
}
