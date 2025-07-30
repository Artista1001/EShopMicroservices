
namespace Catalog.Api.Products.GetProductByCategory
{
    //public record GetProductByCategoryRequest(string Category);
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductByCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/getProductbyCategory/{categoryName}", async(string CategoryName, ISender sender) =>
            {
                var products = await sender.Send(new GetProductByCategoryQuery(CategoryName));
                GetProductByCategoryResponse response = products.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(response);
            })
                .WithName("GetProductByCategory")
                .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithName("Get Product By Category")
                .WithDescription("Get Product by Category");          
              
        }
    }
}
