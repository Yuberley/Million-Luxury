namespace MillionLuxury.Application.Properties.GetProperty;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Properties.Dtos;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Properties;
#endregion

internal sealed class GetPropertyHandler : IQueryHandler<GetPropertyQuery, PropertyResponse>
{
    #region Private Members
    private readonly IPropertyRepository propertyRepository;
    #endregion

    public GetPropertyHandler(IPropertyRepository propertyRepository)
    {
        this.propertyRepository = propertyRepository;
    }

    public async Task<Result<PropertyResponse>> Handle(GetPropertyQuery request, CancellationToken cancellationToken)
    {
        var property = await propertyRepository.GetByIdWithImagesAsync(request.PropertyId, cancellationToken);

        if (property is null)
        {
            return Result.Failure<PropertyResponse>(PropertyErrors.NotFound);
        }

        return Result.Success(property.ToDto());
    }
}
