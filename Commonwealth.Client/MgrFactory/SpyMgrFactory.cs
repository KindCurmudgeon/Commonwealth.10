namespace Commonwealth.Shared.EconomicMgrs;

public static class SpyMgrFactory
{
     public static SpyMgr Create(int nationCode)
     {
          SpyOrder order = new SpyOrder()
          {
               Status = SpyState.Added,
          };
          return new SpyMgr(order, isNew: true)
          {
            NationCode =  nationCode,
          };
     }
}