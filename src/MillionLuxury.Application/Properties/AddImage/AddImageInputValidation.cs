namespace MillionLuxury.Application.Properties.AddImage;

#region Usings
using FluentValidation;
#endregion

public class AddImageInputValidation : AbstractValidator<AddImageCommand>
{
    #region Constants
    private const int MaxFileNameLength = 255;
    private const int MaxBase64ContentLength = 10485760; // 10MB in base64 characters
    #endregion

    public AddImageInputValidation()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty();

        RuleFor(x => x.Request.FileName)
            .NotEmpty()
            .MaximumLength(MaxFileNameLength)
            .Must(HaveValidImageExtension)
            .WithMessage("File must have a valid image extension (.jpg, .jpeg, .png, .webp)");

        RuleFor(x => x.Request.Base64Content)
            .NotEmpty()
            .MaximumLength(MaxBase64ContentLength)
            .Must(BeValidBase64)
            .WithMessage("Invalid base64 content");
    }

    private static bool HaveValidImageExtension(string fileName)
    {
        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return validExtensions.Contains(extension);
    }

    private static bool BeValidBase64(string base64String)
    {
        try
        {
            Convert.FromBase64String(base64String);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
