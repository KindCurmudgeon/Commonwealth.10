using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.Data;

public partial class Spy
{
     public void SeasonUpdate(SpyMgr? spyMgr, VGame vGame, Nation? nation)
     {
          if (spyMgr is null || nation is null) return;
          if (spyMgr.Order.NewDistrict is not null)
          {
               District = spyMgr.Order.NewDistrict;
               Order.NewDistrict = null;
          }

          switch (spyMgr.Order?.Status)
          {
               case SpyState.Registered:
                    CodeName = GetNextCodeName();
                    Order.Status = (District is not null) ? SpyState.Active : SpyState.Inactive;
                    break;
               case SpyState.Inactive:
                    Order.Status = SpyState.Active;
                    break;
               default:
                    break;
          }
          CreateReport();

          int GetNextCodeName()
          {
               int candidate = 1;
               while (nation.Spies?.Find(s => s.CodeName == candidate) is not null) candidate++;
               return candidate;
          }
     void CreateReport()
     {
          Report = null;
          if (Order.Status == SpyState.Active)
          {
               VDistrict? district = vGame.FindDistrict(District!);
               Report = district?.LastSeasonVillageReport;
          }
     }          
     }

}
