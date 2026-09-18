using System.Text.Json.Serialization;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;

namespace Commonwealth.Server.Data;

public partial class WorldSetup : IBlobObject
{
     public required string GameName { get; set; }
     public required List<DistrictSetup> Districts { get; set; }
     [JsonConstructor] public WorldSetup() { }
}

public partial class WorldSetup: IBlobObject
{
     public static WorldSetup Create(string gameName, GeographyFile geogParmsFile, InitParms initParms, EconParms econParms)
     {
          List<DistrictSetup> districts = [];
          CreateDistrictSetups();
          AssignResources();
          IdentifyAllowedVillages();
          return new WorldSetup()
          {
               GameName = gameName,
               Districts = districts
          };

          void CreateDistrictSetups()
          {
               foreach (DistrictParm districtParm in geogParmsFile.Districts)
               {
                    DistrictSetup district = new DistrictSetup(districtParm, geogParmsFile.Seas);
                    districts.Add(district);
               }
          }
          void AssignResources()
          {
               foreach (Resource resource in econParms.Resources)
               {
                    List<DistrictSetup> elgible = districts.Where(s => s.HasFeature(resource.Constraint)).ToList();
                    Util.Shuffle(elgible);
                    int netDistricts = (int)((resource.Existence ?? 1.0) * elgible.Count);
                    while (netDistricts-- > 0)
                    {
                         DistrictSetup district = elgible[0];
                         (district.Resources ??= []).Add(resource.Name);
                         elgible.RemoveAt(0);
                    }
               }
               foreach (DistrictSetup district in districts)
               {
                    int shortfall = initParms.MinResourcePerDistrict - (district.Resources?.Count ?? 0);
                    while (shortfall > 0)
                    {
                         List<Resource> elgible = econParms.Resources.Where(r => district.HasFeature(r.Constraint)).ToList();
                         RemoveExisting(elgible);
                         string? resource = Util.PickRandomFromList<Resource>(elgible)?.Name;
                         if (resource is null) continue;
                         (district.Resources ??= []).Add(resource);
                         shortfall--;
                    }

                    void RemoveExisting(List<Resource> items)
                    {
                         foreach (string resourceName in district.Resources ?? [])
                         {
                              items.RemoveAll(i => i.Name == resourceName);
                         }
                    }
               }
          }


          void IdentifyAllowedVillages()  // SHould this go somewhere else after EconParms.VillageParms are created.
          {
               foreach (DistrictSetup district in districts)
               {
                    foreach (string resource in district.Resources ?? [])
                    {
                         VillageParm? villageParm = econParms.VillageParms.Find(p => p.Resource == resource);
                         district.AllowedVillages ??= [];
                         district.AllowedVillages.AddIfNotNull(villageParm?.Name);
                    }
                    foreach (VillageParm tradingVillageParm in econParms.VillageParms.Where(p => p.Type == VILLAGETYPE.TRADING))
                    {
                         Feature? constraint = tradingVillageParm.Constraint;
                         if (district.HasFeature(constraint))
                         {
                              district.AllowedVillages ??= [];
                              district.AllowedVillages?.Add(tradingVillageParm.Name);
                         }
                    }
               }
          }
     }
}
