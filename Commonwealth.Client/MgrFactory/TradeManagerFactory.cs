using Commonwealth.Shared.EconomicMgrs;

public static class TradeMgrFactory
{
     public static TradeMgr Create(TRADETYPE type)
     {
          TradeOrder order = new TradeOrder()
          {
               Id = Guid.NewGuid(),
               TradeType = type
          };
          return new TradeMgr(order, isNew: true) {};
     }
}