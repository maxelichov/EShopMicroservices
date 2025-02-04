﻿using Catalog.API.Products.GetProducts;

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdRequest(int? PageNumber = 1, int? PageSize = 10);
public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id));
            var response = result.Adapt<GetProductByIdResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProductsById")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products By Id")
        .WithDescription("Get Products By Id");
    }
}
