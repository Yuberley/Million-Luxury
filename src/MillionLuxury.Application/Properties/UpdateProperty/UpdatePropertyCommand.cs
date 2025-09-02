namespace MillionLuxury.Application.Properties.UpdateProperty;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Properties.Dtos;
#endregion

public class UpdatePropertyCommand : ICommand
{
    public Guid PropertyId { get; }
    public UpdatePropertyRequest Request { get; }

    public UpdatePropertyCommand(Guid propertyId, UpdatePropertyRequest request)
    {
        PropertyId = propertyId;
        Request = request;
    }
}
