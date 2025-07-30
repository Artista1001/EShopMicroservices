namespace Catalog.Api.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImangeFile, decimal Price) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Product Id is required");

            RuleFor(commnad => commnad.Name)
                .NotEmpty().WithMessage("Name is Required")
                .Length(2, 150).WithMessage("Name Must be between 2 and 150 characters");

            RuleFor(command => command.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero");
        }
    }
    public class UpdateProductCommandHandler(IDocumentSession documentSession) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await documentSession.LoadAsync<Product>(command.Id, cancellationToken);
            if (product == null) 
            {
                throw new ProductNotFoundException(command.Id);
            }

            product.Name = command.Name;
            product.Category = command.Category;
            product.Description = command.Description;
            product.ImageFile = command.ImangeFile;
            product.Price = command.Price;  

            documentSession.Update(product);
            await documentSession.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(true);
        }
    }
}
