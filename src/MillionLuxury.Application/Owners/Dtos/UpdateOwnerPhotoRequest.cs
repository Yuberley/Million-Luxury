namespace MillionLuxury.Application.Owners.Dtos;

public class UpdateOwnerPhotoRequest
{
    public string FileName { get; init; } = string.Empty;
    public string Base64Content { get; init; } = string.Empty;
}
