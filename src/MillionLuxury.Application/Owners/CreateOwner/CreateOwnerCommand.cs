namespace MillionLuxury.Application.Owners.CreateOwner;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Owners.Dtos;
#endregion

public record CreateOwnerCommand(CreateOwnerRequest Request) : ICommand<Guid>;
