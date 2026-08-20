using Commonwealth.Shared.Common;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Commonwealth.Shared.EconomicMgrs;

public partial class SpyMgr : IMgr<SpyOrder>
{
    public bool HasChanged { get; set; } = false;
    public int Id { get => Order.Id; }
    public int NationCode { get; set; }
    public string Name { get => (Id == 0) ? "Unnamed" : Id.ToString("D3"); }
    public SpyOrder Order { get; private set; }
    public string? District { get; set; }
    public Report? Report { get; set; }
    public List<string> Adjustments { get; set; } = [];
    [JsonConstructor] public SpyMgr(SpyOrder order) { Order = order; }

    [SetsRequiredMembers]
    public SpyMgr(SpyOrder order, string? district, int nationCode, Report? report)
    {
        Order = order;
        District = district;
        NationCode = nationCode;
        Report = report;
    }

    public void ChangeOrder(SpyOrder order)
    {
        Order = order;
        HasChanged = true;
    }
}
public partial class SpyOrder
{
    public int Id { get; set; }
    public SpyState Status { get; set; }
    public SpyAction Action { get; set; }
    public string? NewDistrict { get; set; }

    [JsonConstructor] public SpyOrder() { }
    [SetsRequiredMembers]
    public SpyOrder(int id)
    {
        Id = id;
        NewDistrict = null;
        Status = SpyState.TRAINING;
        Action = SpyAction.TOBEADDED;
    }
    public SpyOrder DeepCopy()
    {
        return new SpyOrder()
        {
            Id = Id,
            Status = Status,
            Action = Action,
            NewDistrict = NewDistrict
        };
    }
}



public class SpyIdentity(int nationCode, int spyId)
{
    public int SpyId { get; set; } = spyId;
    public int NationCode { get; set; } = nationCode;

    public bool Equals(SpyIdentity other)
    {
        if (NationCode != other.NationCode) return false;
        if (SpyId != other.SpyId) return false;
        return true;
    }
    public override string ToString() => SpyId.ToString("D3");
}
public enum SpyState { NONE = 0, TRAINING = 10, ACTIVE = 20, REASSIGNED = 30, INACTIVE = 99 }
public enum SpyAction { NONE = 0, TOBEADDED = 10, TOBEREMOVED = 99 }

public partial class EconomicMgr
{
    public void HandleSpy(SpyMgr spyMgr, NationMgr nationMgr)
    {

        List<Asset>? cost = GetCost(spyMgr);
        (int adjustedCount, REASON reason) = DependentConsume(1, cost, null, nationMgr, null, nameof(EconActivity.Spying), null);
        if (adjustedCount == 0 && spyMgr.Order.Status == SpyState.INACTIVE)
        {
            spyMgr.Order.Status = SpyState.ACTIVE;
        }
        if (adjustedCount > 0)
        {
            spyMgr.Order.Status = SpyState.INACTIVE;
            nationMgr.Adjustments?.Add($"Spy {spyMgr.Name} inactive due to insufficient resources.");
        }

        List<Asset>? GetCost(SpyMgr mgr)
        {
            switch (mgr.Order.Action)
            {
                case SpyAction.TOBEREMOVED: return null;
            }
            switch (mgr.Order.Status)
            {
                case SpyState.INACTIVE: return null;
                case SpyState.TRAINING: return EconParms.SpyParms.Training;
                case SpyState.ACTIVE: return EconParms.SpyParms.Operation;
                case SpyState.REASSIGNED: return EconParms.SpyParms.Transfer;

                default: return null;

            }
        }
    }
}
