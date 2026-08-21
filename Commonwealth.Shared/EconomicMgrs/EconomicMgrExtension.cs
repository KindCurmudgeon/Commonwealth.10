using Commonwealth.Shared.Common;

namespace Commonwealth.Shared.EconomicMgrs;

public partial class EconomicMgr
{
     public (int, REASON) DependentConsume(
                int count,
                List<Asset>? cost,
                EconActivity? goodsSource,
                EconActivity? currencySource,
                EconActivity? staffingSource,
                string account,
                int? staffNeeded = null)
     {
          if (count == 0 || cost is null) return (0, REASON.NONE);
          if (isClient) return Consume(count, cost, goodsSource, currencySource, staffingSource, account, staffNeeded);
          return LimitedConsume(count, cost, goodsSource, currencySource, staffingSource, account, staffNeeded);
     }
     public (int, REASON) Consume(
             int count,
             List<Asset> cost,
             EconActivity? goodsSource,
             EconActivity? currencySource,
             EconActivity? staffSource,
             string account,
             int? staffNeeded)
     {
          while (count > 0)
          {
               if ((staffSource?.StaffRemaining ?? 0) < (staffNeeded ?? 0)) return (count, REASON.STAFFING);
               List<Asset> goodsCost = ExtractAllButTypeFrom(cost, GOODTYPE.CURRENCY);
               List<Asset> currencyCost = ExtractTypeFrom(cost, GOODTYPE.CURRENCY);
               goodsSource?.ConsumeGoods(account, goodsCost);
               currencySource?.ConsumeGoods(account, currencyCost);
               staffSource?.ConsumeStaff(staffNeeded);
               count--;
          }
          return (0, REASON.NONE);
     }
     public (int, REASON) LimitedConsume(
                     int count,
                     List<Asset> cost,
                     EconActivity? goodsSource,
                     EconActivity? currencySource,
                     EconActivity? staffSource,
                     string account,
                     int? staffNeeded)
     {
          while (count > 0)
          {
               List<Asset> goods = ExtractAllButTypeFrom(goodsSource?.Final, GOODTYPE.CURRENCY);
               List<Asset> goodsCost = ExtractAllButTypeFrom(cost, GOODTYPE.CURRENCY);
               if (goods.IsSufficient(goodsCost) is false) return (count, REASON.GOODS);
               List<Asset> currency = ExtractTypeFrom(currencySource?.Final, GOODTYPE.CURRENCY);
               List<Asset> currencyCost = ExtractTypeFrom(cost, GOODTYPE.CURRENCY);
               if (currency.IsSufficient(currencyCost) is false) return (count, REASON.MONEY);
               if ((staffSource?.StaffRemaining ?? 0) < (staffNeeded ?? 0)) return (count, REASON.STAFFING);
               goodsSource?.ConsumeGoods(account, goodsCost);
               currencySource?.ConsumeGoods(account, currencyCost);
               staffSource?.ConsumeStaff(staffNeeded);
               count--;
          }
          return (0, REASON.NONE);
     }
     public List<Asset> ExtractTypeFrom(List<Asset>? source, string goodType)
     {
          List<Asset> result = [];
          foreach (Asset asset in source ?? [])
          {
               GoodParm? parm = EconParms.GoodParms.Find(p => p.Name == asset.Name);
               if (parm?.Type == goodType) result.Add(asset);
          }
          return result;
     }
     public List<Asset> ExtractAllButTypeFrom(List<Asset>? source, string goodType)
     {
          List<Asset> goods = [];
          foreach (Asset asset in source ?? [])
          {
               GoodParm? parm = EconParms.GoodParms.Find(p => p.Name == asset.Name);
               if (parm?.Type != goodType) goods.Add(asset);
          }
          return goods;
     }
     public List<Asset> GetNeededFood(TradeOrder order)
     {
          DistrictMgr? districtMgr = AllDistrictMgrs.Find(d => d.Name == order.DistrictName);
          List<Asset> needed = districtMgr?.FoodMetrics?.AllocatedFoodConsumed ?? [];
          List<Asset> available = ExtractTypeFrom(districtMgr?.Initial, GOODTYPE.FOOD);
          needed.ConsumeWithoutLimits(available);
          needed.ZeroNegatives();
          return needed;
     }
     public List<Asset> GetNeededVillages(TradeOrder order)
     {
          List<Asset> needed = [];
          if (order.SendNeededVillages is true)
          {
               List<VillageMgr> villageMgrsHere = AllVillageMgrs.FindAll(v => v.Identity.Owner == order.SendingNationCode && v.Identity.District == order.DistrictName);
               foreach (VillageMgr villageMgr in villageMgrsHere)
               {
                    VillageParm? parm = EconParms.VillageParms.Find(v => v.Name == villageMgr.Identity.Name);
                    needed.Accumulate(ExtractAllButTypeFrom(parm?.Activation, GOODTYPE.CURRENCY).MultiplyBy(villageMgr.Order.Activate));
                    needed.Accumulate(ExtractAllButTypeFrom(parm?.Construction, GOODTYPE.CURRENCY).MultiplyBy(villageMgr.Order.Build));
                    needed.Accumulate(ExtractAllButTypeFrom(parm?.Operation, GOODTYPE.CURRENCY).MultiplyBy(villageMgr.Active));
               }
          }
          return needed;
     }
     public List<Asset>? GetNetSendWithNeeds(TradeOrder order)
     {
          List<Asset> _send = order.Send ?? [];
          if (order.SendNeededFood is true)
          {
               List<Asset> neededFood = GetNeededFood(order);
               ZeroType(_send, GOODTYPE.FOOD);
               _send.Accumulate(neededFood);
          }
          if (order.SendNeededVillages is true)
          {
               List<Asset> neededVillages = GetNeededVillages(order);
               ZeroType(_send, GOODTYPE.VILLAGE);
               _send.Accumulate(neededVillages);
          }
          return (_send.IsEmpty()) ? null : _send;
     }
     public void ZeroType(List<Asset> source, string goodType)
     {
          foreach (Asset asset in source)
          {
               string? Type = EconParms.GoodParms.Find(p => p.Type == goodType)?.Type;
               if (Type == goodType) asset.Amount = 0;
          }
     }

     public int GetMarketPrice(Asset asset)
     {
          GoodParm? goodParm = EconParms.GoodParms.Find(g => g.Name == asset.Name);
          if (goodParm?.Type == GOODTYPE.CURRENCY) return 1;
          double Price = MarketPrices?.Find(m => m.Name == asset.Name)?.Price ?? 0.0;
          return (int)(Price * (1 - EconParms.MarketParms.Commission ?? 0));
          //    MarketPrice? price = MarketPrices.Find(p => p.Name == asset.Name)
     }
     public static string CreateAjustmentString(int amount, ADJTYPE type, REASON reason)
     {
          string typeString = type switch
          {
               //   ADJTYPE.ACTIVATION => "activation",
               ADJTYPE.OPERATION => "operation",
               ADJTYPE.CONSTRUCTION => "construction",
               _ => "unknown"
          };
          string reasonString = reason switch
          {
               REASON.NONE => "none",
               REASON.STAFFING => "staffing",
               REASON.GOODS => "goods",
               REASON.MONEY => "money",
               _ => "unknown"
          };
          return $"{typeString.Pluralize(amount)} rescinded due to {reasonString}.";
     }
     public VillageParm? FindVillageParm(string? villageName)
     {
          if (villageName is null) return null;
          return EconParms.VillageParms.Find(p => p.Name == villageName);
     }
}
public enum REASON { NONE, STAFFING, GOODS, MONEY }
public record Adjustment(int Amount, ADJTYPE Type, REASON Reason);
public enum ADJTYPE { ACTIVATION, OPERATION, CONSTRUCTION }
public interface IEconActivity
{
     public void Reset();
}
