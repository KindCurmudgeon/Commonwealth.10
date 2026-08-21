using Commonwealth.Shared.Common;
using System.Text.Json.Serialization;

namespace Commonwealth.Shared.EconomicMgrs;

public class TradeMgr : IOrderMgr<TradeOrder>
{
    public Guid Id { get => Order.Id; }
    public bool HasChanged { get; set; } = false;
    public TradeOrder Order { get; private set; }
    public List<string> Adjustments { get; set; } = [];
    [JsonConstructor] public TradeMgr(TradeOrder order, bool isNew = false) { Order = order; HasChanged = isNew; }

    public void ChangeOrder(TradeOrder tradeOrder)
    {
        Order = tradeOrder;
        HasChanged = true;
    }
}
public partial class TradeOrder
{
    public Guid Id { get; set; }
    public TRADETYPE TradeType { get; set; }
    public int SendingNationCode { get; set; }
    public int? ReceiverNationCode { get; set; }
    public string? ReceiverNationName { get; set; }
    public string? DistrictName { get; set; }
    public List<Asset>? Send { get; set; }
    public Asset? Receive { get; set; }
    public bool SendNeededFood { get; set; }
    public bool SendNeededVillages { get; set; }
    public bool IsStandingTrade { get; set; }
    public bool IsNullified { get; set; }
    [JsonConstructor] public TradeOrder() { }
    // public TradeOrder(TRADETYPE tradeType, int sendingNationCode)
    // {
    //     Id = Guid.NewGuid();
    //     SendingNationCode = sendingNationCode;
    //     TradeType = tradeType;
    //     ReceiverNationCode = null;
    //     DistrictName = null;
    //     Send = [];
    //     Receive = null;
    //     SendNeededFood = false;
    //     SendNeededVillages = false;
    //     IsStandingTrade = false;
    //     IsNullified = false;
    // }
    public TradeOrder DeepCopy()
    {
        return new TradeOrder()
        {
            Id = Id,
            SendingNationCode = SendingNationCode,
            TradeType = TradeType,
            ReceiverNationCode = ReceiverNationCode,
            ReceiverNationName = ReceiverNationName,
            DistrictName = DistrictName,
            Send = Send,
            SendNeededFood = SendNeededFood,
            SendNeededVillages = SendNeededVillages,
            Receive = Receive,
            IsStandingTrade = IsStandingTrade,
            IsNullified = IsNullified
        };
    }
    public string? Summary()
    {
        bool isValidTrade = true;
        string? summary = "[]";
        if (Send is null && (SendNeededFood is false || SendNeededVillages is false)) isValidTrade = false;
        else
        {
            switch (TradeType)
            {
                case TRADETYPE.DISTRICT:
                    isValidTrade = DistrictName is not null;
                    string augment = (SendNeededFood is true || SendNeededVillages is true) ? "(as needed)" : "";
                    summary = $"Send {augment}: {Send?.AssetString()} to {DistrictName ?? "[]"}.";
                    break;
                case TRADETYPE.NATION:
                    isValidTrade = ReceiverNationCode is not null;
                    summary = $"Send {Send?.AssetString()} to {ReceiverNationName ?? "[]"}.";
                    break;
                case TRADETYPE.MARKET:
                    //   Receive!.Amount = updater?.GetMarketTradeReceive(this) ?? 0;
                    string rcv = Asset.Exists(Receive) ? $"{Receive!.Amount} {Receive.Name}" : "[]";
                    summary = $"Market Trade: Send {Send?.AssetString()} for {rcv}.";
                    break;
            }
        }
        IsNullified = !isValidTrade;
        return summary;
    }
}

public enum TRADETYPE { NONE, DISTRICT, NATION, MARKET }

public partial class EconomicMgr
{
    public void HandleTradeSend(TradeMgr mgr, NationMgr source)
    {
        TradeOrder? order = mgr.Order;
        if (order is null) return;
        order.Send = GetNetSendWithNeeds(order);
        bool isSufficient = source.Final?.IsSufficient(order.Send) ?? false;
        if (isSufficient is true)
        {
            source.ConsumeGoods(nameof(EconActivity.Trades), order.Send);
        }
        else
        {
            order.IsNullified = true;
            source.Adjustments?.Add($"Failed to send trade order of {order.Send?.AssetString()} due to insufficient resources.");
        }
        // (int adjustedCount, REASON reason) = DependentConsume(1, send, source);
        // if (adjustedCount > 0)
        // {
        //     order.IsNullified = true;
        //     source.Activities?.Add($"Failed to send trade order of {order.Send.Name} to due to insufficient resources.");
        //     return;
        // }
    }

    public void HandleTradeReceive(TradeMgr mgr, EconActivity source)
    {
        TradeOrder? order = mgr.Order;
        //  if (order?.Receive is null) return;
        if (order.IsNullified == true) return;
        EconActivity? target = null;
        switch (order.TradeType)
        {
            case TRADETYPE.NATION:
                target = AllNationMgrs.Find(n => n.NationCode == mgr.Order.ReceiverNationCode);
                target?.AddGoods(nameof(EconActivity.Trades), order.Send);
                break;
            case TRADETYPE.DISTRICT:
                target = AllDistrictMgrs.Find(d => d.Name == mgr.Order.DistrictName);
                target?.AddGoods(nameof(EconActivity.Trades), order.Send);
                break;
            case TRADETYPE.MARKET:
                target = source;
                if (order.Receive is null) break;
                target.AddGoods(nameof(EconActivity.Trades), new List<Asset>() { order.Receive });
                break;
            default: break;
        }
    }
    public void SetMarketTradeReceive(TradeOrder order)
    {
        if (order.Receive is null) return;
        double sendPrice = 0;
        foreach (Asset asset in order.Send ?? [])
        {
            double price = GetMarketPrice(asset);
            sendPrice += price * asset.Amount;
        }

        double? receivePrice = MarketPrices?.Find(m => m.Name == order.Receive?.Name)?.Price;
        if (receivePrice == null || sendPrice == 0) return;
        double nominalReceiveAmount = sendPrice / (double)receivePrice;
        double adjustedReceiveAmount = nominalReceiveAmount * (1 - EconParms.MarketParms.Commission ?? 0);
        order.Receive.Amount = (int)adjustedReceiveAmount;
    }
}