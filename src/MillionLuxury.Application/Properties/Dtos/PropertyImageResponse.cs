namespace MillionLuxury.Application.Properties.Dtos;

public record PropertyImageResponse(
    Guid Id,
    string FileName,
    string FilePath,
    bool IsEnabled,
    DateTime CreatedAt
);
