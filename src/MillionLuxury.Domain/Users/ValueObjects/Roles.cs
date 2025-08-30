namespace MillionLuxury.Domain.Users.ValueObjects;

public record Roles
{
    private static readonly Roles Admin = new(["Admin"]);

    public IReadOnlyCollection<string> Values { get; }

    private Roles(IReadOnlyCollection<string> values)
    {
        if (values == null || !values.Any())
            throw new ArgumentNullException(nameof(values), "Roles cannot be null or empty.");

        Values = values;
    }

    public static Roles Create(params string[] values)
    {
        var validRoles = new List<string>();

        foreach (var value in values)
        {
            var roleValue = value switch
            {
                "Admin" => "Admin",
                _ => throw new ArgumentException($"Invalid role value: {value}")
            };

            validRoles.Add(roleValue);
        }

        return new Roles(validRoles);
    }

    public static bool Exists(string[] roles)
    {
        return roles.All(role => All.Any(r => r.Values.Contains(role)));
    }

    public static string[] NotExist(string[] roles)
    {
        return roles.Where(role => !All.Any(r => r.Values.Contains(role))).ToArray();
    }

    public static readonly IReadOnlyCollection<Roles> All = new[]
    {
        Admin,
    };
}