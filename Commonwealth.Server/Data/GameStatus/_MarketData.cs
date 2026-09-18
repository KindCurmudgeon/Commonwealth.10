using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.Data;

public class MarketData
{
    [JsonInclude] public required string Name { get; set; }
    [JsonInclude] public int Inventory { get; private set; }
    [JsonInclude] public int NominalInventory { get; private set; }
    [JsonInclude] public double Price { get; private set; }

    [JsonConstructor] public MarketData() { }
    [SetsRequiredMembers]
    public MarketData(MarketInit init)
    {
        Name = init.Name;
        NominalInventory = init.NominalInventory;
        AdjustInventory(init.Inventory);
        UpdatePrice();
    }
    public void AdjustInventory(int amount)
    {
        Inventory += amount;
    }
    public void UpdatePrice()
    {
        Price = Math.Round((double)NominalInventory / Inventory, 2, MidpointRounding.AwayFromZero);
    }
    public void SeasonUpdate(List<TradeMgr> AllTradeMgrs, List<MarketData> marketDatas)
    {
        List<TradeMgr> marketTrades = AllTradeMgrs.Where(t => t.Order?.TradeType == TRADETYPE.MARKET).ToList();
        foreach (TradeMgr tradeMgr in marketTrades)
        {
            TradeOrder? order = tradeMgr.Order;
            if (order is null) continue;
            if (order.IsNullified == true) continue;
            foreach (Asset asset in tradeMgr.Order.Send ?? [])
            {
                //           MarketData? marketData = Game.MarketDatas?.Find(m => m.Name == asset.Name);
                //           if (marketData is null) continue;
                AdjustInventory(asset.Amount);
            }
            MarketData? marketData1 = marketDatas?.Find(m => m.Name == order.Receive?.Name);
            marketData1?.AdjustInventory(-order.Receive!.Amount);
        }
        UpdatePrice();
    }
}
public class MarketInit
{
    public required string Name { get; set; }
    [JsonInclude] public int NominalInventory { get; private set; }
    [JsonInclude] public int Inventory { get; private set; }


    [JsonConstructor] public MarketInit() { }
    [SetsRequiredMembers]
    public MarketInit(string name, int inventory, int nominalInventory)
    {
        Name = name;
        NominalInventory = nominalInventory;
        Inventory = inventory;
    }
    // public void UpdateInventory(int amount)
    // {
    //     Inventory = amount;
    //     Price = Math.Round((double)NominalInventory / Inventory, 2);
    // }
}