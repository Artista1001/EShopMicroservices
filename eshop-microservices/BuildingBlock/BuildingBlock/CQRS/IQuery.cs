using MediatR;

namespace BuildingBlock.CQRS
{
    public interface IQuery<out TResponse>: IRequest<TResponse> where TResponse : notnull;
        //  IQuery Interface design to return result which shoould not be null and will be use for read Operation
}
