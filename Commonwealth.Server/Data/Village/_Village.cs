using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Shared.EconomicMgrs;


namespace Commonwealth.Server.Data;

public partial class Village
{
    public VillageIdentity Identity { get => Order.Identity; }
    public required VillageOrder Order { get; set; }
    public int Count { get; set; }
    public int Active { get; set; }
    [JsonConstructor] public Village() { }
    // [SetsRequiredMembers]
    // public Village(VillageOrder order, int count, int active)
    // {
    //     Order = order;
    //     Count = count;
    //     Active = active;
    // }
    [SetsRequiredMembers]
    public Village(VillageMgr mgr)
    {
        Order = mgr.Order;
        Count = mgr.Count;
        Active = mgr.Active;
    }

}
