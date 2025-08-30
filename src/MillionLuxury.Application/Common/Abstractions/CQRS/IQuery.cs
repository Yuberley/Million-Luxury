namespace MillionLuxury.Application.Common.Abstractions.CQRS;

using MediatR;
using MillionLuxury.Domain.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{

}