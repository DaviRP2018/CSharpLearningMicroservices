namespace Catalog.Api.Models;

/// <summary>
///     Represents a product in the catalog.
///     This is the core domain model (and currently the data model for Marten)
///     that represents the data we store and retrieve.
///     It is defined as a class with properties to allow for state changes
///     and compatibility with ORMs/Document stores.
/// </summary>
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<string> Category { get; set; } = new();
    public string Description { get; set; } = null!;
    public string ImageFile { get; set; } = null!;
    public decimal Price { get; set; }
}