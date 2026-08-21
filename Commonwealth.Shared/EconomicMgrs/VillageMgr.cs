using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Shared.EconomicMgrs;

public class VillageMgr : IOrderMgr<VillageOrder>
{
    public bool HasChanged { get; set; } = false;
    public VillageIdentity Identity { get => Order.Identity; }
    public VillageOrder Order { get; private set; }
    public int NetActive { get => Active + Order!.Activate - Order!.Deactivate; }
    public List<string> Adjustments { get; set; } = [];
    public int WorkersAssigned { get; set; }
    public double StaffingScore { get; set; }
    public int Count { get; set; }
    public int Active { get; set; }
    [JsonConstructor] public VillageMgr(VillageOrder order) { Order = order; }

    // [SetsRequiredMembers]
    // public VillageMgr(VillageOrder order, int? count = null, int? active = null)
    // {
    //     Order = order;
    //     Count = count ?? 0;
    //     Active = active ?? 0;
    //     Adjustments = [];
    // }
    public void ChangeOrder(VillageOrder order)
    {
        Order = order;
        HasChanged = true;
    }
    public string Summary(List<NationNaming> namings)
    {
        string? nationPssv = namings.Find(n => n.NationCode == Identity.Owner)?.Possessive;
        string x = $"{nationPssv} {Identity.Name} Village";
        return x.Pluralize(Count);
    }

}
public class VillageOrder
{
    public required VillageIdentity Identity { get; init; }
    public int Build { get; set; }
    public int Activate { get; set; }
    public int Deactivate { get; set; }
    public int PremiumWages { get; set; }
    [JsonConstructor] public VillageOrder() { }
    [SetsRequiredMembers]
    public VillageOrder(VillageIdentity identity)
    {
        Identity = identity;
        Build = 0;
        Activate = 0;
        Deactivate = 0;
        PremiumWages = 0;
    }
    public VillageOrder DeepCopy()
    {
        return new VillageOrder()
        {
            Identity = new VillageIdentity(Identity.District, Identity.Name, Identity.Owner),
            Build = Build,
            Activate = Activate,
            Deactivate = Deactivate,
            PremiumWages = PremiumWages
        };
    }
}

public partial class VillageIdentity
{
    public required string District { get; set; }
    public required string Name { get; set; }
    public required int Owner { get; set; }
    public bool Equals(VillageIdentity other)
    {
        if (Name != other.Name) return false;
        if (District != other.District) return false;
        if (Owner != other.Owner) return false;
        return true;
    }
    [SetsRequiredMembers]
    public VillageIdentity(string district, string name, int owner)
    {
        District = district;
        Name = name;
        Owner = owner;
    }
}

public partial class EconomicMgr
{
    public void VillageActivation(VillageMgr villageMgr, DistrictMgr districtMgr, NationMgr nationMgr)
    {
        List<Asset>? cost = FindVillageParm(villageMgr.Identity.Name)?.Activation;
        int villageCount = villageMgr.Order!.Activate;
        (int alteredVillages, REASON reason) = DependentConsume(villageCount, cost, districtMgr, nationMgr, districtMgr, nameof(EconActivity.Villages), null);
        if (alteredVillages > 0)
        {
            villageMgr.Order.Activate -= alteredVillages;
            villageMgr.Adjustments.Add(CreateAjustmentString(alteredVillages, ADJTYPE.ACTIVATION, reason));
        }
    }
    public void VillageOperation(VillageMgr villageMgr, DistrictMgr districtMgr, NationMgr nationMgr)
    {
        VillageParm? parm = FindVillageParm(villageMgr.Identity.Name);
        List<Asset>? cost = parm?.Operation;
        int? staffNeeded = parm?.Workers;
        switch (districtMgr.FoodMetrics?.FoodStatus)
        {
            case FoodStatus.RATIONING:
                break;
            case FoodStatus.FAMINE:
                break;
        }
        (int alteredVillages, REASON reason) = DependentConsume(villageMgr.NetActive, cost, districtMgr, nationMgr, districtMgr, nameof(EconActivity.Villages), staffNeeded);
        if (alteredVillages > 0)
        {
            villageMgr.Order.Deactivate += alteredVillages;
            villageMgr.Adjustments.Add(CreateAjustmentString(alteredVillages, ADJTYPE.OPERATION, reason));
        }
    }
    public void VillageConstruction(VillageMgr villageMgr, DistrictMgr districtMgr, NationMgr nationMgr)
    {
        List<Asset>? cost = FindVillageParm(villageMgr.Identity.Name)?.Construction;
        int villageCount = villageMgr.Order!.Build;
        (int alteredVillages, REASON reason) = DependentConsume(villageCount, cost, districtMgr, nationMgr, districtMgr, nameof(EconActivity.Villages), null);
        if (alteredVillages > 0)
        {
            villageMgr.Order.Build -= alteredVillages;
            villageMgr.Adjustments.Add(CreateAjustmentString(alteredVillages, ADJTYPE.CONSTRUCTION, reason));
        }
    }
    public void VillageProduction(VillageMgr villageMgr, NationMgr nationMgr)
    {
        if (villageMgr.Active <= 0) return;
        Asset? production = EconParms.VillageParms.Find(v => v.Name == villageMgr.Identity.Name)?.Production;
        if (production is null) return;
        production = production.MultiplyBy(villageMgr.Active);
        nationMgr.AddGoods(nameof(EconActivity.Production), new List<Asset>() { production });
        // HandleTaxes();

        // void HandleTaxes()
        // {
        //     if (Production is null || Production.Amount == 0) return;
        //     DistrictMgr? districtMgr =  nationMgr.DistrictMgrs.Find(d => d.Name == villageMgr.Identity.District);
        //     double taxRate = districtMgr?.Order?.TaxRate ?? 0;
        //     if (taxRate == 0.0) return;
        //     if (districtMgr?.Owner == 0) return;
        //     if (villageMgr.Identity.Owner == districtMgr?.Owner) return;

        //     Asset taxes = new(Production.Name, (int)Math.Floor(Production.Amount * taxRate));
        //     if (nationMgr.GoodsTaxPaid is null) nationMgr.GoodsTaxPaid = [];
        //     nationMgr.GoodsTaxPaid.Accumulate(taxes);
        //     if (nationMgr.GoodsNet is null) nationMgr.GoodsNet = [];
        //     nationMgr.GoodsNet.ConsumeWithoutLimits(taxes);

        //     NationMgr? taxingNation = NationMgrs.Find(n => n.NationCode == districtMgr?.Owner);
        //     if (taxingNation is null) return;
        //     if (taxingNation.GoodsNet is null) taxingNation.GoodsNet = [];
        //     taxingNation.GoodsNet?.Accumulate(taxes);
        //     taxes.Amount = -taxes.Amount;
        //     if (taxingNation.GoodsTaxPaid is null) taxingNation.GoodsTaxPaid = [];
        //     taxingNation.GoodsTaxPaid?.Accumulate(taxes);
        // }
    }
}

public static class VillageMgrExpansion
{
    public static List<VillageMgr> Here(this List<VillageMgr> source, string? districtName)
    {
        return source.Where(r => r.Identity.District == districtName).ToList();
    }
    public static List<VillageMgr> OwnedBy(this List<VillageMgr> source, int? nationCode)
    {
        if (nationCode is null || nationCode is 0) return [];
        return source.Where(v => v.Identity.Owner == nationCode).ToList();
    }

}