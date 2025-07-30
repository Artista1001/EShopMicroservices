namespace Basket.Api.Basket.DeleteBasket
{
    public record DeleteBasketResponse(bool IsSuccess);
    public class DeleteBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async(string userName, ISender sender) =>
            {
                var IsSuccess = await sender.Send(new DeleteBasketCommand(userName));

                return Results.Ok(IsSuccess);
            })
            .WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Basket")
            .WithDescription("Delete Basket");
        }
    }
}
