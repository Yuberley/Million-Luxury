namespace MillionLuxury.Application.Properties.ChangePrice;


#region Usings
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.SharedValueObjects;
#endregion

internal sealed class ChangePriceHandler : ICommandHandler<ChangePriceCommand>
{
    #region Private Members
    private readonly IPropertyRepository propertyRepository;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IUnitOfWork unitOfWork;
    #endregion

    public ChangePriceHandler(
        IPropertyRepository propertyRepository,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork)
    {
        this.propertyRepository = propertyRepository;
        this.dateTimeProvider = dateTimeProvider;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ChangePriceCommand request, CancellationToken cancellationToken)
    {
        var property = await propertyRepository.GetByIdAsync(request.PropertyId, cancellationToken);

        if (property is null)
        {
            return Result.Failure(PropertyErrors.NotFound);
        }

        var newPrice = new Money(request.Request.Price, Currency.FromCode(request.Request.Currency));

        property.ChangePrice(newPrice, dateTimeProvider.UtcNow);

        propertyRepository.Update(property);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
