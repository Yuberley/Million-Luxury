namespace MillionLuxury.Application.Properties.CreateProperty;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Properties.Dtos;
#endregion

public record CreatePropertyCommand(CreatePropertyRequest Request) : ICommand<Guid>;