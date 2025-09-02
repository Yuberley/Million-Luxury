namespace MillionLuxury.Application.Properties.GetProperty;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Properties.Dtos;
#endregion

public record GetPropertyQuery(Guid PropertyId) : IQuery<PropertyResponse>;
