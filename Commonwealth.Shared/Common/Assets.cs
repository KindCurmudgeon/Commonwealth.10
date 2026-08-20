using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;


namespace Commonwealth.Shared.Common;

public class Asset
{
    public required string Name { get; set; }
    public int Amount { get; set; }
    [JsonConstructor] public Asset() { }
    [SetsRequiredMembers]
    public Asset(Asset asset)
    {
        Name = asset.Name;
        Amount = asset.Amount;
    }
    [SetsRequiredMembers]
    public Asset(string name, int amount)
    {
        Name = name;
        Amount = amount;
    }
    //public static readonly Asset Error = new("Error", 0);
    public override string ToString()
    {
        return $"{Amount} {Name}";
    }

    public static string FormatAmount(int? amount)
    {
        if (amount is null || amount == 0) return "-";
        string suffix = "";
        decimal amt = (decimal)amount!;
        if (amount >= 1000)
        {
            amt /= 1000;
            suffix = "K";
        }
        if (amount >= 1000000)
        {
            amt /= 1000000;
            suffix = "M";
        }
        //  double d = Math.Truncate(amt);
        decimal d = RoundToSignificantDigits(amt, 3);
        if (d == (int)d) return ((int)d).ToString() + suffix;
        return (d).ToString() + suffix;

        static decimal RoundToSignificantDigits(decimal d, int digits)
        {
            decimal scale = (decimal)Math.Pow(10, Math.Floor(Math.Log10((double)Math.Abs(d))) + 1 - digits);
            return scale * Math.Truncate(d / scale);
        }
    }
    public static bool Exists(Asset? asset)
    {
        if (asset is null) return false;
        if (string.IsNullOrWhiteSpace(asset.Name)) return false;
        return asset.Amount != 0;
    }
}
public static class AssetExtenstions
{
    public static Asset? GetAsset(this List<Asset> assets, string name)
    {
        Asset? asset = assets.Find(a => a.Name == name);
        return asset;
    }
    public static Asset DeepCopy(this Asset asset)
    {
        return new Asset(asset.Name, asset.Amount);
    }
    public static void SetAsset(this List<Asset> assets, Asset? asset)
    {
        if (asset is null) return;
        Asset? existing = assets.Find(a => a.Name == asset.Name);
        if (existing is null) assets.Add(new Asset(asset));
        else existing.Amount = asset.Amount;
    }
    public static bool IsEmpty(this List<Asset> assets)
    {
        foreach (Asset asset in assets) if (asset.Amount != 0) return false;
        return true;
    }

    public static Asset MultiplyBy(this Asset asset, int? multiple)
    {
        int amount = asset.Amount * (multiple ?? 0);
        return new Asset(asset.Name, amount);
    }
    public static List<Asset> MultiplyBy(this List<Asset> assets, int? multiple)
    {
        List<Asset> answer = assets.DeepCopy();
        foreach (Asset asset in answer) asset.Amount *= (multiple ?? 0);
        return answer;
    }
    public static List<Asset> MultiplyByDouble(this List<Asset> assets, double multiple)
    {
        List<Asset> answer = assets.DeepCopy();
        foreach (Asset asset in answer) asset.Amount = (int)Math.Round(multiple * asset.Amount);
        return answer;
    }
    public static List<Asset> DeepCopy(this List<Asset> assets)
    {
        List<Asset> newList = [];
        foreach (Asset asset in assets) newList.Add(new Asset(asset.Name, asset.Amount));
        return newList;
    }
    public static bool IsSufficient(this List<Asset> source, List<Asset>? proposedConsume)
    {
        foreach(Asset consumeAsset in proposedConsume ?? [])
        {
            Asset? sourceAsset = source.Find(t => t.Name == consumeAsset.Name);
            if (consumeAsset.Amount > (sourceAsset?.Amount ?? 0)) return false;
        }
        return true;
    }

    public static bool ConsumeIfSufficient(this List<Asset> source, List<Asset>? consumption)
    {
        List<Asset> sourceCopy = source.DeepCopy();
        bool sufficient = true;
        foreach (Asset consumeAsset in consumption ?? [])
        {
            Asset? sourceAsset = source.Find(t => t.Name == consumeAsset.Name);
            if (sourceAsset is null) continue;
            sourceAsset.Amount -= consumeAsset.Amount;
            if (sourceAsset.Amount < 0)
            {
                sufficient = false;
                break;
            }
        }
        if (sufficient is false) source = sourceCopy;
        return sufficient;
    }

    public static void ConsumeWithoutLimits(this List<Asset> source, Asset? consumption)
    {
        if (consumption is null) return;
        Asset? sourceAsset = source.Find(t => t.Name == consumption.Name);
        if (sourceAsset is null) source.Add(new Asset(consumption.Name, -consumption.Amount));
        else sourceAsset.Amount -= consumption.Amount;
    }
    public static void ConsumeWithoutLimits(this List<Asset> source, List<Asset>? consumption)
    {
        foreach (Asset consumeAsset in consumption ?? [])
        {
            Asset? sourceAsset = source.Find(t => t.Name == consumeAsset.Name);
            if (sourceAsset is null) source.Add(new Asset(consumeAsset.Name, -consumeAsset.Amount));
            else sourceAsset.Amount -= consumeAsset.Amount;
        }
    }

    public static void Accumulate(this List<Asset> source, Asset? addition)
    {
        if (addition is null) return;
        Asset? sourceAsset = source.Find(a => a.Name == addition.Name);
        if (sourceAsset is null) source.Add(new Asset(addition));
        else
        {
            sourceAsset.Amount += addition.Amount;
        }
    }
    public static void Accumulate(this List<Asset> source, List<Asset>? added)
    {
        foreach (Asset asset in added ?? [])
        {
            Asset? sourceAsset = source.Find(a => a.Name == asset.Name);
            if (sourceAsset is null) source.Add(new Asset(asset));
            else
            {
                sourceAsset.Amount += asset.Amount;
            }
        }
    }
    public static List<Asset> ZeroNegatives(this List<Asset> source)
    {
        List<Asset> zeroed = [];
        foreach (Asset asset in source) if (asset.Amount > 0) zeroed.Add(asset);
        return zeroed;
    }


public static List<Asset> GetNegative(this List<Asset> source)
    {
        List<Asset> negated = [];
        foreach (Asset asset in source) negated.Add(new Asset(asset.Name, -asset.Amount));
        return negated;
    }
    public static (Asset Owner, Asset Tax) SplitAssets(this Asset source, decimal TaxRate)
    {
        int tax = (int)Math.Floor(source.Amount * TaxRate);
        int retain = source.Amount - tax;
        return (new Asset(source.Name, retain), new Asset(source.Name, tax));
    }

    public static string AssetString(this List<Asset> assets)
    {
        if (assets.Count == 0) return "(none)";
        StringBuilder stringBuilder = new StringBuilder();
        foreach (Asset asset in assets)
        {
            if (asset.Amount == 0) continue;
            string element = $"{asset.Amount} {asset.Name}, ";
            stringBuilder.Append(element);
        }
        if (stringBuilder.Length > 0) stringBuilder.Length -= 2; // Remove the last comma and space
        return (stringBuilder.Length == 0) ? "(none)": stringBuilder.ToString();
    }

}