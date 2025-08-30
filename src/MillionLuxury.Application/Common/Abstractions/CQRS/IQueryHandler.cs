namespace MillionLuxury.Application.Common.Abstractions.CQRS;

using MediatR;
using MillionLuxury.Domain.Abstractions;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{

}