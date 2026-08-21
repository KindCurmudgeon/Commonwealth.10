using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization;
using Commonwealth.Shared.Common;

namespace Commonwealth.Shared.EconomicMgrs;

public class EconActivity
{
    [JsonInclude] public List<Asset>? Initial { get; private set; }
    [JsonInclude] public List<Asset>? Trades { get; private set; }
    [JsonInclude] public List<Asset>? Food { get; private set; }
    [JsonInclude] public List<Asset>? Villages { get; private set; }
    [JsonInclude] public List<Asset>? Production { get; private set; }
    [JsonInclude] public List<Asset>? Spying { get; private set; }
    [JsonInclude] public List<Asset>? Final { get; private set; }
    [JsonInclude] public int? StaffRemaining { get; private set; }
    
    [JsonConstructor] public EconActivity(){}
    [SetsRequiredMembers] public EconActivity(List<Asset>? goods, int? staffAvailable)
    {
        Initial = goods?.DeepCopy();
        ResetEconActivity(staffAvailable);
    }
    protected void ResetEconActivity(int? staffAvailable = null)
    {
        Trades = [];
        Food = [];
        Villages = [];
        Production = [];
        Spying = [];
        Final = Initial?.DeepCopy();
        StaffRemaining = staffAvailable;
    }

    public bool ConsumeGoods(string accountName, List<Asset>? amount)
    {
        if (amount == null) return false;
        PropertyInfo? accountProperty = GetAccount(accountName);
        if (accountProperty is null) return false;
        List<Asset>? current = (List<Asset>?)accountProperty.GetValue(this);
        (current ?? []).ConsumeWithoutLimits(amount);
        (Final ?? []).ConsumeWithoutLimits(amount);
        return true;
    }
    public bool ConsumeStaff(int? amount)
    {
        if (StaffRemaining is null) return false;
        StaffRemaining -= amount;
        return true;
    }
    public bool AddGoods(string accountName, List<Asset>? goods)
    {
        if (goods == null) return false;
        PropertyInfo? accountProperty = GetAccount(accountName);
        if (accountProperty is null) return false;
        List<Asset>? current = (List<Asset>?)accountProperty.GetValue(this);
        (current ?? []).Accumulate(goods);
        (Final ??= []).Accumulate(goods);
        return true;
    }
    private PropertyInfo? GetAccount(string accountName)
    {
        if (string.IsNullOrWhiteSpace(accountName)) return null;
        PropertyInfo? propertyInfo = GetType().GetProperty(accountName, BindingFlags.Public | BindingFlags.Instance);
        if (propertyInfo is null) return null;
        return propertyInfo;
    }


    public List<RowData> CreateTableRows(List<string>? goodNames)
    {
        List<RowData> rows = [];
        if (HasActivity()) rows.Add(new RowData("Initial", CreateItems(Initial)));
        if (HasContent(Trades)) rows.Add(new RowData("Traded", CreateItems(Trades)));
        if (HasContent(Food)) rows.Add(new RowData("Food", CreateItems(Food)));
        if (HasContent(Villages)) rows.Add(new RowData("Villages", CreateItems(Villages)));
        if (HasContent(Production)) rows.Add(new RowData("Produced", CreateItems(Production)));
        if (HasContent(Spying)) rows.Add(new RowData("Spying", CreateItems(Spying)));
        //if (GoodsTaxPaid is not null && GoodsTaxPaid.IsEmpty() is false) rows.Add(new RowData("Taxes", GoodsTaxPaid.AssetStrings()));
        rows.Add(new RowData("Net", CreateItems(Final)));
        return rows;

        bool HasContent(List<Asset>? assetList)
        {
            if (assetList is null) return false;
            if (assetList.Count == 0) return false;
            return true;
        }
        bool HasActivity()
        {
            if (HasContent(Trades)) return true;
            if (HasContent(Food)) return true;
            if (HasContent(Villages)) return true;
            if (HasContent(Production)) return true;
            if (HasContent(Spying)) return true;
            return false;
        }

        List<string> CreateItems(List<Asset>? assets)
        {
            List<string> items = [];
            foreach (string goodName in goodNames ?? [])
            {
                Asset? asset = assets?.Find(a => a.Name == goodName);
                items.Add(asset?.Amount.ToString() ?? "-");
            }
            return items;
        }
    }
}
