namespace MillionLuxury.Application.Properties.UpdateProperty;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Properties.Dtos;
#endregion

public record UpdatePropertyCommand(Guid PropertyId, UpdatePropertyRequest Property) : ICommand;