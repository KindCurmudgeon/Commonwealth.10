using Commonwealth.Server.Data;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Shared.EconomicMgrs;

public static class MgrExtenstions
{

    public static List<NationNaming> AssembleNationNamings(this List<Nation> nations)
    {
        return nations.Select(n => n.Naming).ToList();
    }
    public static List<MarketPrice> AssembleMarketPrices(this List<MarketData>? marketDatas)
    {
        List<MarketPrice> prices = [];
        foreach (MarketData data in marketDatas ?? [])
        {
            prices.Add(MarketPriceCreator(data));
        }
        return prices;
    }
    public static MarketPrice MarketPriceCreator(MarketData data)
    {
        return new MarketPrice()
        {
            Name = data.Name,
            Price = data.Price,
        };
    }
}



