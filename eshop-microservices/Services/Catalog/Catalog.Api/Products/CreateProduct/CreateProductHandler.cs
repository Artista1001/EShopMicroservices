namespace Catalog.Api.Products.CreateProduct
{ 
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price): ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is Required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is Required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is Required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
        }
    }
    // Why we are injecting the IDocumentSession directly here why we are not creating IRepository pattern or folder or any data folder
    // because IDocumentSession is an already an absraction of database operation so we dont need any additional abstraction or unnecessary code like repository patterns
    internal class CreateProductCommandHandler(IDocumentSession documentSession) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        // here CreateProductCommandHandler handles the request which is comming in CreateProductCommand
        // ICommandHandler was CustomInterface created for Command Operation which is direct implementation of IRequestHandler which is interface of MediatR lib
        // this IRequestHandler is interface to implement CreateProductCommand ( which is a request ICommand:IRequest )
        // means CreateProductCommand is a request handled by CreateProductCommandHandler

        /*
         * Mediator pattern to promote loose coupling between different parts of your application
         * MediatR is a simple mediator library for .NET. 
         * It helps implement the Mediator pattern to promote loose coupling between different parts of your application.
         * Instead of calling services or methods directly, you send requests and let MediatR dispatch them to the appropriate handler.
         * */

        /*
         * "Hey MediatR, this is a request (CreateProductCommand) that should be handled (by CreateProductCommandHandler), and I expect this kind of result. (CreateProductResult)"
         **/
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Business Logic to create Product


            // 1. Create Product entity from commnad object
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };
            // 2. save to database
            documentSession.Store(product);
            await documentSession.SaveChangesAsync(cancellationToken);
            

            // 3. return CreateProductResult as result  
            return new CreateProductResult(product.Id);

        }
    }
}
