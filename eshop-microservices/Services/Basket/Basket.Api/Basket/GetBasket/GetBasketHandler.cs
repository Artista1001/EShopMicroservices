namespace Basket.Api.Basket.GetBasket
{
    public record GetBasketResult(ShopingCart Cart);
    public record GetBasketQuery(string UserName): IQuery<GetBasketResult>;
    public class GetBasketHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {   
        public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var basket = await repository.GetBasket(request.UserName);
            return new GetBasketResult(basket);

        }
    }
}
