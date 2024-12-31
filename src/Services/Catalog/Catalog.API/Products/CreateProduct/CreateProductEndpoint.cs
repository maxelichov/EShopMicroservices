namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest
{
    public string Name { get; init; }
    public List<string> Category {  get; init; }
    public string Description { get; init; }
    public string ImageFile { get; init; }
    public decimal Price { get; init; }
};

public record CreateProductResponse
{
    public Guid Id { get; init; }
};

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateProductResponse>();

            return Results.Created($"/products/{response.Id}", response);

        })
        .WithName("CreateProducts")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Products")
        .WithDescription("Create Products");
    }
}
