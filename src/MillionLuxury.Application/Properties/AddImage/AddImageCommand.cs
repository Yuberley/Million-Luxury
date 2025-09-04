namespace MillionLuxury.Application.Properties.AddImage;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Properties.Dtos;
#endregion

public record AddImageCommand(Guid PropertyId, AddImageRequest AddImage) : ICommand<Guid>;