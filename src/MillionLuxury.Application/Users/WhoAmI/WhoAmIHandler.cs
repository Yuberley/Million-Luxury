namespace MillionLuxury.Application.Users.WhoAmI;

#region Usings
using MillionLuxury.Application.Common.Abstractions.Authentication;
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Users;
#endregion

internal sealed class WhoAmIHandler : IQueryHandler<WhoAmIUserQuery, Dtos.User>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public WhoAmIHandler(
        IUserRepository userRepository,
        IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<Result<Dtos.User>> Handle(
        WhoAmIUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Dtos.User>(UserErrors.NotFound);
        }

        Dtos.User userDto = user.ToDto();

        return userDto;
    }
}