using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Data
{
    public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {
        public async Task<ShopingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
            if(!string.IsNullOrEmpty(cachedBasket)) return JsonSerializer.Deserialize<ShopingCart>(cachedBasket)!;
            var basket = await repository.GetBasket(userName, cancellationToken);
            await cache.SetStringAsync(userName,JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }

        public async Task<ShopingCart> StoreBasket(ShopingCart shopingCart, CancellationToken cancellationToken = default)
        {
            await repository.StoreBasket(shopingCart, cancellationToken);
            await cache.SetStringAsync(shopingCart.UserName,JsonSerializer.Serialize(shopingCart), cancellationToken);
            return shopingCart;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasket(userName, cancellationToken);
            await cache.RemoveAsync(userName, cancellationToken);
            return true;
        }
    }
}
