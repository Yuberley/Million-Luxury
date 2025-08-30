namespace MillionLuxury.Infrastructure.Clock;

using MillionLuxury.Application.Common.Abstractions.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    #region Constants
    private const int UTCColombiaOffset = -5;
    #endregion

    public DateTime UtcNow => DateTime.UtcNow.ToUniversalTime().AddHours(UTCColombiaOffset);
}