namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategotyResult>;
public record GetProductByCategotyResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategotyResult>
{
    public async Task<GetProductByCategotyResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>()
        .Where(p => p.Category.Contains(query.Category))
        .ToListAsync(cancellationToken);

        return new GetProductByCategotyResult(products); ;
    }
}
