namespace MillionLuxury.Application.Properties.Dtos;

public record AddImageRequest(
    string FileName,
    string Base64Content
);
