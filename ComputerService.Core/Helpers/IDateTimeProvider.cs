using System;

namespace ComputerService.Core.Helpers
{
    public interface IDateTimeProvider
    {
        DateTime GetDateTimeNow();
    }
}
