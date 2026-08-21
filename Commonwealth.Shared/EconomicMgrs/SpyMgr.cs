using Commonwealth.Shared.Common;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Commonwealth.Shared.EconomicMgrs;

public class SpyMgr : IOrderMgr<SpyOrder>
{
    public Guid Id { get; set; }
    public bool HasChanged { get; set; }
    public int NationCode { get; set; }
    public int NameCode { get; set; }
    public SpyOrder Order { get => _order; }
    [JsonRequired] private SpyOrder _order { get; set; }
    public string? District { get; set; }
    public Report? Report { get; set; }
    public List<string> Adjustments { get; set; } = [];
    public string Name { get => NameCode.ToString("D3"); }

    [JsonConstructor, SetsRequiredMembers]
    public SpyMgr(SpyOrder order, bool isNew = false)
    {
        Id = Guid.NewGuid();
        _order = order;
        HasChanged = isNew;
    }

    public void ChangeOrder(SpyOrder order)
    {
        _order = order;
        HasChanged = true;
    }
}

public class SpyOrder
{
    //  public int Id { get; set; }
    public SpyState Status { get; set; }
    //    public SpyAction Action { get; set; }
    public string? NewDistrict { get; set; }

    public SpyOrder DeepCopy()
    {
        return new SpyOrder()
        {
            //      Id = Id,
            Status = Status,
            //       Action = Action,
            NewDistrict = NewDistrict
        };
    }
}


// public class SpyIdentity(int nationCode, int spyId)
// {
//     public int SpyId { get; set; } = spyId;
//     public int NationCode { get; set; } = nationCode;

//     public bool Equals(SpyIdentity other)
//     {
//         if (NationCode != other.NationCode) return false;
//         if (SpyId != other.SpyId) return false;
//         return true;
//     }
//     public override string ToString() => SpyId.ToString("D3");
// }
public enum SpyState { NONE = 0, Added = 10, Registered = 20, Active = 30, Inactive = 90, ToBeRetired = 99 }
//public enum SpyAction { NONE = 0, TOBEADDED = 10, TOBEREMOVED = 99 }

public partial class EconomicMgr
{
    public void HandleSpy(SpyMgr spyMgr, NationMgr nationMgr)
    {
        List<Asset>? cost = GetCost(spyMgr);
        (int adjustedCount, REASON reason) = DependentConsume(1, cost, null, nationMgr, null, nameof(EconActivity.Spying), null);
        if (adjustedCount == 0 && spyMgr.Order.Status == SpyState.Inactive)
        {
            spyMgr.Order.Status = SpyState.Active;
        }
        if (adjustedCount > 0)
        {
            spyMgr.Order.Status = SpyState.Active;
            nationMgr.Adjustments?.Add($"Spy {spyMgr.Name} inactive due to insufficient resources.");
        }

        List<Asset>? GetCost(SpyMgr mgr)
        {
            switch (mgr.Order.Status)
            {
                case SpyState.ToBeRetired:
                    return null;
                case SpyState.Added:
                case SpyState.Registered:
                    return EconParms.SpyParms.Training;
                case SpyState.Active:
                    return (mgr.Order.NewDistrict is null) ? EconParms.SpyParms.Operation :EconParms.SpyParms.Transfer;
                case SpyState.Inactive:
                default:
                    return null;

            }
        }
    }
}
