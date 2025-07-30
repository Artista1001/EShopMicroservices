namespace Basket.Api.Models
{
    public class ShopingCart
    {
        public string UserName { get; set; } = default!;
        public List<ShopingCartItem> Items { get; set; } =  new List<ShopingCartItem>();
        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
        public ShopingCart(string username)
        {
            UserName = username;
        }

        // Required for mapping
        public ShopingCart()
        {
            
        }
    }
}
