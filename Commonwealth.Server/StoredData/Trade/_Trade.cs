
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.Data;

public partial class Trade
{
    public Guid Id {get;set;}
    public required TradeOrder Order { get; set; }
    [JsonConstructor] public Trade(){}
    
    // [SetsRequiredMembers]
    // public Trade(TradeOrder tradeOrders)
    // {
    //     Order = tradeOrders;
    // }

    [SetsRequiredMembers]  public Trade(TradeMgr mgr)
    {
        Id = mgr.Id;
        Order = mgr.Order;
    }
}
