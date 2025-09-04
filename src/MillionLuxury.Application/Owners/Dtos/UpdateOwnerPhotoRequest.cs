namespace MillionLuxury.Application.Owners.Dtos;

public class UpdateOwnerPhotoRequest
{
    public string FileName { get; init; }
    public string Base64Content { get; init; }
}
