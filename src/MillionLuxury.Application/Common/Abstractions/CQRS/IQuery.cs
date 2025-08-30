using MediatR;
using RideHubb.Domain.Abstractions;

namespace MillionLuxury.Application.Common.Abstractions.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{

}