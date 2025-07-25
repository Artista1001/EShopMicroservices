
namespace Catalog.Api.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id): ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);
    public class DeleteProductCommandHandler(IDocumentSession documentSession, ILogger<DeleteProductCommandHandler> logger)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteProductCommandHandler.Handler is called with {@model}", command);
            var product = await documentSession.LoadAsync<Product>(command.Id, cancellationToken);
            if (product != null)
            {
                throw new ProductNotFoundException();
            }

            documentSession.Delete(command.Id);
            await documentSession.SaveChangesAsync();
            return new DeleteProductResult(true);
        }
    }
}
