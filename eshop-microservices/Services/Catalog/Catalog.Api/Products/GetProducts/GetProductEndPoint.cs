
namespace Catalog.Api.Products.GetProducts
{
    public record GetProductRequest(int? PageNumber = 1, int? PageSize = 10);
    public record GetProductResponse(IEnumerable<Product> products);
    public class GetProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/productGetAll", async ([AsParameters] GetProductRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetProductQuery>();
                var result = await sender.Send(query);

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
