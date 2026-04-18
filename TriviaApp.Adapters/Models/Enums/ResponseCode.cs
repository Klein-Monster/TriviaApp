using System;
using System.Collections.Generic;
using System.Text;

namespace TriviaApp.Adapter.Models.Enums
{
    public enum ResponseCode
    {
        Success = 0,
        NoResults = 1,
        InvalidParameter = 2,
        TokenNotFound = 3,
        TokenEmpty = 4,
        RateLimit = 5
    }
}
