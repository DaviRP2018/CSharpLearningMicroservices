namespace Catalog.Api.Products.CreateProduct;

/// <summary>
///     Represents a command to create a new product in the catalog.
///     Unlike a query, a command is intended to change the state of the system.
///     This record contains all the necessary data to initialize a new <see cref="Product" />.
/// </summary>
/// <param name="Name">The name of the product.</param>
/// <param name="Category">The categories the product belongs to.</param>
/// <param name="Description">A brief description of the product.</param>
/// <param name="ImageFile">The filename or path to the product's image.</param>
/// <param name="Price">The price of the product.</param>
public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

/// <summary>
///     Represents the result of a <see cref="CreateProductCommand" />.
///     Returning the unique identifier of the newly created entity allows the caller to reference it.
/// </summary>
/// <param name="Id">The unique identifier (GUID) of the created product.</param>
public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

/// <summary>
///     The handler responsible for processing the <see cref="CreateProductCommand" />.
///     It contains the logic to map the command data into a <see cref="Product" /> entity
///     and persist it to the database using <see cref="IDocumentSession" />.
///     This keeps the write-side logic separate and maintainable.
/// </summary>
/// <param name="session">Marten IDocumentSession used for persisting the new product.</param>
internal class
    CreateProductCommandHandler(
        IDocumentSession session,
        IValidator<CreateProductCommand> validator)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        // Business logic to create a product

        var result = await validator.ValidateAsync(command, cancellationToken);
        var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
        if (errors.Count != 0)
            throw new ValidationException(errors.FirstOrDefault());

        // Create a Product entity from a command object
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        // Save the product to the database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        // Return the ID of the created product
        return new CreateProductResult(product.Id);
    }
}