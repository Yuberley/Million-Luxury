namespace MillionLuxury.Application.Owners.DeleteOwner;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
#endregion

public record DeleteOwnerCommand(Guid OwnerId) : ICommand;
