
using MediatR;

namespace BuildingBlock.CQRS
{
    public interface ICommandHandler<in TCommand>
        : ICommandHandler<TCommand, Unit>
        where TCommand: ICommand<Unit>; // will use this interface if we do not return response
 
    public interface ICommandHandler<in TCommand, TResponse> : 
        IRequestHandler<TCommand, TResponse> 
        where TCommand: ICommand<TResponse> 
        where TResponse : notnull; // will use this interface if we do return response
}
