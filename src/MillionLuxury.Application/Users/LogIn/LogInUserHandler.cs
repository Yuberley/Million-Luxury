namespace MillionLuxury.Application.Users.LogIn;

#region Usings
using MillionLuxury.Application.Common.Abstractions.Authentication;
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Users;
#endregion

internal sealed class LogInUserHandler : ICommandHandler<LogInUserCommand, AccessTokenResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IHashingService _hashingService;

    public LogInUserHandler(IUserRepository userRepository, IJwtProvider jwtProvider, IHashingService hashingService)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _hashingService = hashingService;
    }

    public async Task<Result<AccessTokenResponse>> Handle(LogInUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            return Result.Failure<AccessTokenResponse>(UserErrors.NotFound);
        }

        if (!_hashingService.VerifyPassword(user.PasswordHash, request.Password))
        {
            return Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials);
        }

        var token = _jwtProvider.GenerateToken(user);

        return Result.Success(new AccessTokenResponse(token));
    }
}