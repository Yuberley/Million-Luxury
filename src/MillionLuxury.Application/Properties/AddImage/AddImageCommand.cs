namespace MillionLuxury.Application.Properties.AddImage;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Properties.Dtos;
#endregion

public class AddImageCommand : ICommand<Guid>
{
    public Guid PropertyId { get; }
    public AddImageRequest Request { get; }

    public AddImageCommand(Guid propertyId, AddImageRequest request)
    {
        PropertyId = propertyId;
        Request = request;
    }
}
