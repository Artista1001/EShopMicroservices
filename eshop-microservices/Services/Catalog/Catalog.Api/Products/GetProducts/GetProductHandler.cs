
namespace Catalog.Api.Products.GetProducts
{
    public record GetProductResult(IEnumerable<Product> Products);
    public record GetProductQuery() : IQuery<GetProductResult>;
    internal class GetProductQueryHandler(IDocumentSession documentSession, ILogger<GetProductQueryHandler> logger) : IQueryHandler<GetProductQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductQuery model, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductQueryHandler.Handle is called {@Model}", model);

            var products = await documentSession.Query<Product>().ToListAsync(cancellationToken);
            return new GetProductResult(products);

        }
    }
}
