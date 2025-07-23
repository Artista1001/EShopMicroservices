
namespace Catalog.Api.Products.GetProducts
{
    
    public record GetProductResponse(IEnumerable<Product> products);
    public class GetProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/productGetAll", async (ISender sender) =>
            {
                var result = await sender.Send(new GetProductQuery());

                GetProductResponse response = result.Adapt<GetProductResponse>();

                return response;

            })
            .WithName("GetProducts")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
        }
    }
}
