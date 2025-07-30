namespace Basket.Api.Data
{
    public interface IBasketRepository
    {
        Task<ShopingCart> GetBasket(string userName, CancellationToken cancellationToken = default);
        Task<ShopingCart> StoreBasket(ShopingCart shopingCart, CancellationToken cancellationToken = default);
        Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);
    }
}
