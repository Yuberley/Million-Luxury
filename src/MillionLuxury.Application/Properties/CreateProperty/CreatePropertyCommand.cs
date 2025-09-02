namespace MillionLuxury.Application.Properties.CreateProperty;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Properties.Dtos;
#endregion

public class CreatePropertyCommand : ICommand<Guid>
{
    public CreatePropertyRequest Request { get; }

    public CreatePropertyCommand(CreatePropertyRequest request)
    {
        Request = request;
    }
}
