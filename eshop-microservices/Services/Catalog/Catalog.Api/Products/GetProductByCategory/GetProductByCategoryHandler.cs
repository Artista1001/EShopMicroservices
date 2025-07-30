
namespace Catalog.Api.Products.GetProductByCategory
{
    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    public record GetProductByCategoryQuery(string Category): IQuery<GetProductByCategoryResult>;
    public class GetProductByCategoryQueryHandler(IDocumentSession documentSession) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await documentSession.Query<Product>()
                            .Where(x => x.Category.Contains(request.Category)).ToListAsync();
            return new GetProductByCategoryResult(products);
        }
    }
}