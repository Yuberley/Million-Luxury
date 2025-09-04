namespace MillionLuxury.Application.Properties.CreateProperty;

#region Usings
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Properties;
#endregion

internal sealed class CreatePropertyHandler : ICommandHandler<CreatePropertyCommand, Guid>
{
    #region Private Members
    private readonly IPropertyRepository propertyRepository;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IUnitOfWork unitOfWork;
    #endregion

    public CreatePropertyHandler(
        IPropertyRepository propertyRepository,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork)
    {
        this.propertyRepository = propertyRepository;
        this.dateTimeProvider = dateTimeProvider;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        if (await propertyRepository.ExistsByInternalCodeAsync(request.Request.InternalCode, cancellationToken))
        {
            return Result.Failure<Guid>(PropertyErrors.InternalCodeAlreadyExists(request.Request.InternalCode));
        }

        var property = request.Request.ToDomain(dateTimeProvider.UtcNow);

        propertyRepository.Add(property);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(property.Id);
    }
}
