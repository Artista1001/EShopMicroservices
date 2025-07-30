namespace Catalog.Api.Products.GetProductById 
{
    public record GetProductByIdResult(Product Product);
    public record GetProductByIdQuery(Guid Id): IQuery<GetProductByIdResult>;

    public class GetProductByIdValidator: AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdValidator()
        {
            RuleFor(query => query.Id).NotEmpty().WithMessage("Id is required");
        }
    }
    internal class GetProductByIdQueryHandler(IDocumentSession documentSession) 
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery model, CancellationToken cancellationToken)
        {
            var result = await documentSession.LoadAsync<Product>(model.Id, cancellationToken);            
            if (result == null) {
                throw new ProductNotFoundException(model.Id);
            } else {
                return new GetProductByIdResult(result);    
            }
        }
    }
}