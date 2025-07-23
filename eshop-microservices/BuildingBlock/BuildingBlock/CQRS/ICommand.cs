using MediatR;

namespace BuildingBlock.CQRS
{
    public interface ICommnad : ICommand<Unit>; // Interface of Command which does not return response
    public interface ICommand<out TResponse> : IRequest<TResponse>; // Interface of command that return Generic Response
}
