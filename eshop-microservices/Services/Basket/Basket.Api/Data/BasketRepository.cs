namespace Basket.Api.Data
{
    public class BasketRepository(IDocumentSession documentSession) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            documentSession.Delete<ShopingCart>(userName);
            await documentSession.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<ShopingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await documentSession.LoadAsync<ShopingCart>(userName, cancellationToken);
            return basket is null ? throw new BasketNotFoundException(userName): basket;
        }

        public async Task<ShopingCart> StoreBasket(ShopingCart shopingCart, CancellationToken cancellationToken = default)
        {
            documentSession.Store(shopingCart);
            await documentSession.SaveChangesAsync(cancellationToken);
            return shopingCart;
        }
    }
}
