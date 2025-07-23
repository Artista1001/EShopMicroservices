namespace Catalog.Api.Products.GetProductById 
{
    public record GetProductByIdResult(Product Product);
    public record GetProductByIdQuery(Guid Id): IQuery<GetProductByIdResult>;
    internal class GetProductByIdQueryHandler(IDocumentSession documentSession, ILogger<GetProductByIdQueryHandler> logger) 
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery model, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByIdQueryHandler.Handle called with {@Model}", model);
            var result = await documentSession.LoadAsync<Product>(model.Id, cancellationToken);            
            if (result == null) {
                throw new ProductNotFoundException();
            } else {
                return new GetProductByIdResult(result);
            }
        }
    }
}