namespace MillionLuxury.Application.Properties.ChangePrice;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Properties.Dtos;
#endregion

public class ChangePriceCommand : ICommand
{
    public Guid PropertyId { get; }
    public ChangePriceRequest Request { get; }

    public ChangePriceCommand(Guid propertyId, ChangePriceRequest request)
    {
        PropertyId = propertyId;
        Request = request;
    }
}
