namespace MillionLuxury.Application.Properties.ChangePrice;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Properties.Dtos;
#endregion

public record ChangePriceCommand(Guid PropertyId, ChangePriceRequest ChangePrice) : ICommand;