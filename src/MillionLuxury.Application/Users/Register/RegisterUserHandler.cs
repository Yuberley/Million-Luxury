namespace MillionLuxury.Application.Users.Register;

#region Usings
using MillionLuxury.Application.Common.Abstractions.Authentication;
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Users;
using MillionLuxury.Domain.Users.ValueObjects;
#endregion

internal sealed class RegisterUserHandler : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IHashingService _hashingService;

    public RegisterUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IHashingService hashingService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _hashingService = hashingService;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (existingUser is not null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        var hashedPassword = _hashingService.HashPassword(request.Password);

        if (Roles.NotExist(request.Roles).Any())
        {
            return Result.Failure<Guid>(UserErrors.RolesDoNotExist(Roles.NotExist(request.Roles)));
        }

        var user = User.Register(
            request.Email,
            hashedPassword,
            Roles.Create(request.Roles),
            _dateTimeProvider.UtcNow
        );

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}