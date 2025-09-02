namespace MillionLuxury.Application.Properties.SearchProperties;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Properties.Dtos;
#endregion

public class SearchPropertiesQuery : IQuery<SearchPropertiesResponse>
{
    public SearchPropertiesRequest Request { get; }

    public SearchPropertiesQuery(SearchPropertiesRequest request)
    {
        Request = request;
    }
}
