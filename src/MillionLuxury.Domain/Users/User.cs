namespace MillionLuxury.Domain.Users;

#region Usings
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.User.Events;
using MillionLuxury.Domain.Users.ValueObjects; 
#endregion

public sealed class User : Entity
{
    private User(
        Guid userId,
        string email,
        string passwordHash,
        Roles roles,
        bool isEmailVerified,
        DateTime createdAt
        ) : base(userId)
    {
        Email = email;
        PasswordHash = passwordHash;
        Roles = roles;
        IsEmailVerified = isEmailVerified;
        CreatedAt = createdAt;
    }

    private User() { }

    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public Roles Roles { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static User Register(
        string email,
        string passwordHash,
        Roles roles,
        DateTime createdAt
        )
    {
        var user = new User(
            Guid.NewGuid(),
            email,
            passwordHash,
            roles,
            false,
            createdAt
        );

        user.AddDomainEvent(new UserRegisterDomainEvent(user.Id));

        return user;
    }
}
