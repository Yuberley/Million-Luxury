namespace MillionLuxury.Application.Owners.UpdateOwnerPhoto;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Owners.Dtos;
#endregion

public record UpdateOwnerPhotoCommand(Guid OwnerId, UpdateOwnerPhotoRequest Request) : ICommand;
