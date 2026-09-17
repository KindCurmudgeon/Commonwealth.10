
using Commonwealth.Server.Data;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.ServerEconomics;

public partial class ServerEconomicMgr
{
    public void Immigration()
    {
        List<Movement> movements = GetMovements();
        UpdatePopulations();


        List<Movement> GetMovements()
        {
            List<Movement> Movements = [];
            foreach (DistrictMgr mgr in AllDistrictMgrs)
            {
                DistrictSetup districtSetup = WorldSetup.GetDistrict(mgr.Name);

                foreach (string destination in districtSetup.Connections)
                {
                    DistrictMgr? target = AllDistrictMgrs.Find(e => e.Name == destination);
                    if (target is null) continue;
                    double delta = target.FoodMetrics?.SeasonsOfFood ?? 0 - mgr.FoodMetrics?.SeasonsOfFood ?? 0;
                    if (delta <= 0) continue;
                    int amount = (int)Math.Floor(delta * EconParms.ImmigrationFactor);
                 //   VDistrict? targetDistrict = VGame.FindVDistrict(target.Name);
                 //   if (targetDistrict is null) continue;
                    Movements.Add(new Movement(mgr, target, amount));
                }
            }
            return Movements;
        }

        void UpdatePopulations()
        {
            foreach (DistrictMgr mgr in AllDistrictMgrs)
            {
                List<Movement> fromDistrict = movements.Where(m => m.Source.Name == mgr.Name).OrderByDescending(m => m.Amount).ToList();
                foreach (Movement movement in fromDistrict)
                {
                    movement.Emmigration();
                }
            }
            foreach (DistrictMgr mgr in AllDistrictMgrs)
            {
                List<Movement> toDistrict = movements.Where(m => m.Target.Name == mgr.Name).ToList();
                foreach (Movement movement in toDistrict)
                {
                    movement.Immigration();
                }
            }
        }
    }
    private class Movement(DistrictMgr source, DistrictMgr target, int amount)
    {
        public DistrictMgr Source = source;
        public DistrictMgr Target = target;
        public int Amount = amount;

        public void Emmigration()
        {
            if (Amount > Source.Population) Amount = Source.Population ?? 0;
            Source.Population -= Amount;
            Target.Population += Amount;
        }
        public void Immigration()
        {
            Source.Population -= Amount;
            Target.Population += Amount;
        }
    }
}


