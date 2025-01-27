namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart ShoppingCart);

public class GetBasketQueryHandler(IBasketRepository respository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await respository.GetBasket(query.UserName);

        if (basket == null)
        {
            throw new BasketNotFoundException(query.UserName);
        }

        return new GetBasketResult(basket);
    }
}
