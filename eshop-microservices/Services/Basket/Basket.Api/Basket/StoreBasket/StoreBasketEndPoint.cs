namespace Basket.Api.Basket.StoreBasket
{
    public record StoreBasketRequest(ShopingCart cart);
    public record StoreBasketResponse(ShopingCart cart);
    public class StoreBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/{userName}", async (StoreBasketRequest request, ISender sender) =>
            {
                StoreBasketCommand command = request.Adapt<StoreBasketCommand>();
                var result = await sender.Send(command);
                StoreBasketResponse response = result.Adapt<StoreBasketResponse>();
                return Results.Created($"/basket/{response.cart.UserName}", response);
            })
            .WithName("StoreBasket")
            .Produces<StoreBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Store Basket")
            .WithDescription("Store Basket");
        }
    }
}
