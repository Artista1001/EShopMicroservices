namespace Basket.Api.Basket.StoreBasket
{
    public record StoreBasketResult(ShopingCart cart);
    public record StoreBasketCommand(ShopingCart cart): ICommand<StoreBasketResult>;
    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    public class StoreBasketHandler(IBasketRepository repository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await repository.StoreBasket(command.cart, cancellationToken);
            return new StoreBasketResult(basket);
        }
    }
}
