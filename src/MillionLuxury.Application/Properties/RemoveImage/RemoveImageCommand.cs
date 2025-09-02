namespace MillionLuxury.Application.Properties.RemoveImage;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
#endregion

public record RemoveImageCommand(Guid PropertyId, Guid ImageId) : ICommand;
