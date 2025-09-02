namespace MillionLuxury.Application.Properties.SearchProperties;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Properties.Dtos;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.Properties.ValueObjects;
#endregion

internal sealed class SearchPropertiesHandler : IQueryHandler<SearchPropertiesQuery, SearchPropertiesResponse>
{
    #region Private Members
    private readonly IPropertyRepository propertyRepository;
    #endregion

    public SearchPropertiesHandler(IPropertyRepository propertyRepository)
    {
        this.propertyRepository = propertyRepository;
    }

    public async Task<Result<SearchPropertiesResponse>> Handle(SearchPropertiesQuery request, CancellationToken cancellationToken)
    {
        var searchResult = await propertyRepository.SearchAsync(
            request.Request.Page,
            request.Request.PageSize,
            request.Request.MinPrice,
            request.Request.MaxPrice,
            request.Request.Status.HasValue ? (PropertyStatus)request.Request.Status.Value : null,
            cancellationToken
        );

        var response = searchResult.ToSearchResponse(request.Request.Page, request.Request.PageSize);

        return Result.Success(response);
    }
}
