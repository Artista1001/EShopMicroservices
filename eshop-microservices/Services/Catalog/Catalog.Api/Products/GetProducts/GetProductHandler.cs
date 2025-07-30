
using Marten.Pagination;

namespace Catalog.Api.Products.GetProducts
{

    public record GetProductResult(IEnumerable<Product> Products);
    public record GetProductQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductResult>;
    internal class GetProductQueryHandler(IDocumentSession documentSession) : IQueryHandler<GetProductQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
        {
            var products = await documentSession.Query<Product>()
                                .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10,cancellationToken);
            return new GetProductResult(products);

        }
    }
}
