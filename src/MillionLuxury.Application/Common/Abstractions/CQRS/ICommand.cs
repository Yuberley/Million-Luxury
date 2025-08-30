using MediatR;
using RideHubb.Domain.Abstractions;

namespace MillionLuxury.Application.Common.Abstractions.CQRS;

public interface IBaseCommand
{

}

public interface ICommand : IRequest<Result>, IBaseCommand
{

}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{

}