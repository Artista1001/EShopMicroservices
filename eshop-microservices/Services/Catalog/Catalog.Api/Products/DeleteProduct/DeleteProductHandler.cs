
namespace Catalog.Api.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id): ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductResultValidator: AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductResultValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Product Id is required");
        }
    }
    public class DeleteProductCommandHandler(IDocumentSession documentSession)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
           
            var product = await documentSession.LoadAsync<Product>(command.Id, cancellationToken);
            if (product != null)
            {
                throw new ProductNotFoundException(command.Id);
            }

            documentSession.Delete(command.Id);
            await documentSession.SaveChangesAsync();
            return new DeleteProductResult(true);
        }
    }
}
