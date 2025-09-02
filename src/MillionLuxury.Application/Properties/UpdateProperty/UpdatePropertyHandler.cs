namespace MillionLuxury.Application.Properties.UpdateProperty;

#region Usings
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.Properties.ValueObjects;
#endregion

internal sealed class UpdatePropertyHandler : ICommandHandler<UpdatePropertyCommand>
{
    #region Private Members
    private readonly IPropertyRepository propertyRepository;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IUnitOfWork unitOfWork;
    #endregion

    public UpdatePropertyHandler(
        IPropertyRepository propertyRepository,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork)
    {
        this.propertyRepository = propertyRepository;
        this.dateTimeProvider = dateTimeProvider;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
    {
        var property = await propertyRepository.GetByIdAsync(request.PropertyId, cancellationToken);

        if (property is null)
        {
            return Result.Failure(PropertyErrors.NotFound);
        }

        property.UpdateProperty(
            request.Request.Name,
            request.Request.Address.ToDomain(),
            request.Request.Year,
            (PropertyStatus)request.Request.Status,
            request.Request.Details.ToDomain(),
            dateTimeProvider.UtcNow
        );

        propertyRepository.Update(property);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
