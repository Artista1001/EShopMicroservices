namespace Catalog.Api.Products.CreateProduct
{

    public record CreateProductRequest(string Name, List<string> Category, string Description, string ImangeFile, decimal Price);
    public record CreateProductResponse(Guid Id);
    public class CreateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductRequest model, ISender sender) =>
            {
                CreateProductCommand command = model.Adapt<CreateProductCommand>();

                var result = await sender.Send(command);

                CreateProductResponse response = result.Adapt<CreateProductResponse>();

                return Results.Created($"/products/{response.Id}", response);
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
        }
    }
}
