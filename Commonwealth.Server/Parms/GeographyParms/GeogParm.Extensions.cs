namespace Commonwealth.Server.Parameters;

public static class ParmsGeographyExtensions
{

    // public static void AssignResources(this List<DistrictParm> districtParms, List<Resource> resources)
    // {
    //     foreach (Resource resourceFile in resources)
    //     {
    //         if (resourceFile.Name is null) continue;
    //         Feature constraint = Util.StringToEnum<Feature>(resourceFile.Constraint);
    //         List<DistrictParm> elgible = districtParms.Where(s => s.HasFeature(constraint)).ToList();
    //         Util.Shuffle(elgible);
    //         int netDistricts = (int)((resourceFile.Existence ?? 1.0) * elgible.Count);
    //         while (netDistricts-- > 0)
    //         {
    //             DistrictParm districtParm = elgible[0];
    //             districtParm.Resources?.Add(resourceFile.Name);
    //             //      districtParm.Features?.Add(resource.Name);
    //             elgible.RemoveAt(0);
    //         }
    //     }
    // }

    // public static void IdentifyAllowedVillages(this List<DistrictParm> districtParms, List<VillageParm> villageParms)  // SHould this go somewhere else after EconParms.VillageParms are created.
    // {
    //     foreach (DistrictParm districtParm in districtParms)
    //     {
    //         foreach (VillageParm villageParm in villageParms)
    //         {
    //             if (IsVillageAllowed(districtParm, villageParm))
    //             {
    //                 districtParm.AllowedVillages?.AddIfNotNull(villageParm?.Name);
    //             }
    //         }

    //         bool IsVillageAllowed(DistrictParm? districtParm, VillageParm? villageParm)
    //         {
    //             if (villageParm is null) return false;
    //             if (districtParm is null) return false;
    //             if (villageParm.Type == VILLAGETYPE.PRODUCTION)
    //             {
    //                 return districtParm.IsProductionVillageAllowed(villageParm.Resource);
    //             }
    //             if (villageParm.Type == VILLAGETYPE.TRADING)
    //             {
    //                 return districtParm.IsTradingVillageAllowed(villageParm.Constraint);
    //             }
    //             return false;
    //         }
    //     }
    // }
    // public static List<Region> ConstructRegionParms(this List<RegionFile> regionParmsRaw)
    // {
    //     List<Region> parms = [];
    //     foreach (RegionFile regionRaw in regionParmsRaw ?? [])
    //     {
    //         parms.Add(new Region(regionRaw));
    //     }
    //     return parms;
    // }
    // public static List<DistrictParm> ConstructDistrictParms(this List<DistrictParmFile> districtParmsRaw)
    // {
    //     List<DistrictParm> parms = [];
    //     foreach (DistrictParmFile districtRaw in districtParmsRaw)
    //     {
    //         parms.Add(new DistrictParm(districtRaw));
    //     }
    //     return parms;
    // }
    // public static void ConstructConnections(this List<DistrictParm> districtParms, List<DistrictParmFile>? districtsRaw, List<DistrictParmFile>? seasRaw)
    // {
    //     foreach (DistrictParm districtParm in districtParms)
    //     {
    //         DistrictParmFile? rawParm = districtsRaw?.Find(d => d.Name == districtParm.Name);
    //         districtParm.CreateLandPartners(rawParm);
    //         districtParm.CreateSeaPartners(rawParm, seasRaw, 1);
    //     }
    // }
    // public static void CreateLandPartners(this DistrictParm districtParm, DistrictParmFile? districtParmRaw)
    // {
    //     foreach (string connection in districtParmRaw?.LandConnections ?? [])
    //     {
    //         districtParm.LandPartners?.Add(connection);
    //     }
    // }
    // public static void CreateSeaPartners(this DistrictParm districtParm, DistrictParmFile? sourceParm, List<DistrictParmFile>? seas, int depthLevel)
    // {
    //     List<string> seasChecked = [];

    //     foreach (string first in sourceParm?.SeaConnections ?? [])
    //     {
    //         ProcessSeaLayer(first, depthLevel);
    //     }
    //     void ProcessSeaLayer(string seaName, int depth)
    //     {
    //         if (depth <= 0) return;
    //         seasChecked.Add(seaName);
    //         DistrictParmFile? seaParm = seas?.Find(s => s.Name == seaName);
    //         foreach (string connection in seaParm?.LandConnections ?? [])
    //         {
    //             if (connection == sourceParm?.Name) continue;
    //             if (districtParm.SeaPartners?.Contains(connection) is not false) continue;
    //             districtParm.SeaPartners.Add(connection);
    //         }
    //         foreach (string connection in seaParm?.SeaConnections ?? [])
    //         {
    //             if (seasChecked.Contains(connection)) continue;
    //             ProcessSeaLayer(connection, depth - 1);
    //         }

    //     }
    // }
    // public static DistrictParm? Find(this List<DistrictParm> source, string name)
    // {
    //     return source.Find(p => p.Name == name);
    // }
    // public static int GetHomeRegionCount(this GameParms parms, string homeRegion)
    // {
    //     return parms.Geography.DistrictParms.Where(d => d.Region == homeRegion).ToList().Count;
    // }
}