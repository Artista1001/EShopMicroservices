
namespace Catalog.Api.Products.UpdateProduct
{
    public record UpdateProductRequest(Product product);
    public record UpdateProductResponse(bool IsSuccess);
    public class UpdateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/UpdateProduct", async (UpdateProductRequest request, ISender sender) =>
            {
                UpdateProductCommand updatecommand = request.Adapt<UpdateProductCommand>();
                var isSuccess = await sender.Send(updatecommand);
                UpdateProductResponse response = isSuccess.Adapt<UpdateProductResponse>();
                return response;
            })
            .WithName("UdpateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Udpate Product")
            .WithDescription("Update Product");
        }
    }
}
