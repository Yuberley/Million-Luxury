namespace MillionLuxury.Domain.Properties;

#region Usings
using MillionLuxury.Domain.Abstractions;
#endregion

/// <summary>
/// Represents an image associated with a property
/// </summary>
public sealed class PropertyImage : Entity
{
    private PropertyImage(
        Guid imageId,
        Guid propertyId,
        string fileName,
        string filePath,
        bool isEnabled,
        DateTime createdAt
    ) : base(imageId)
    {
        PropertyId = propertyId;
        FileName = fileName;
        FilePath = filePath;
        IsEnabled = isEnabled;
        CreatedAt = createdAt;
    }

    private PropertyImage() { }

    public Guid PropertyId { get; private set; }
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public bool IsEnabled { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static PropertyImage Create(
        Guid imageId,
        Guid propertyId,
        string fileName,
        string filePath,
        DateTime createdAt
    )
    {
        return new PropertyImage(
            imageId,
            propertyId,
            fileName,
            filePath,
            true,
            createdAt
        );
    }

    public void Disable()
    {
        IsEnabled = false;
    }

    public void Enable()
    {
        IsEnabled = true;
    }
}
