namespace Basket.Api.Basket.DeleteBasket
{
    public record DeleteBasketResult(bool isSucess);
    public record DeleteBasketCommand(string userName): ICommand<DeleteBasketResult>;
    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.userName).NotEmpty().WithMessage("UserName is required");
        }
    }
    public class DeleteBasketHandler(IBasketRepository repository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            // Dalete Basket from database and cache
            var isSuccess = await repository.DeleteBasket(request.userName, cancellationToken);
            return new DeleteBasketResult(isSuccess);
        }
    }
}
