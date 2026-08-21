using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Commonwealth.Shared.EndpointDTOs;

public class NationRequest : RequestBase
{
    public NationRequestType RequestType { get; set; }
    public NationIdentity? Identity { get; set; }
    public NationNaming? Naming { get; set; }
    public string? HomeDistrict { get; set; }
    public bool? IsAccepted { get; set; }

}
public class NationResponse : ResponseBase
{
    public NationIdentity? Identity { get; set; }
    public NationNaming? Naming { get; set; }
    public string? HomeDistrict { get; set; }
    public List<string>? AvailableHomes { get; set; }
    public bool? IsGameActivated { get; set; }
}
public class NationIdentity
{
    public required string GameName { get; set; }
    public required int NationCode { get; set; }
    [JsonConstructor] public NationIdentity() { }
    [SetsRequiredMembers]
    public NationIdentity(string gameName, int nationCode)
    {
        GameName = gameName;
        NationCode = nationCode;
    }
    public bool IsSameAs(NationIdentity? other)
    {
        if (other == null) return false;
        return (other.GameName == GameName && other.NationCode == NationCode);
    }

}
public class NationNaming
{
    public int NationCode { get; set; }
    public string? Name { get; set; }
    public string? Possessive { get; set; }
    public string? LeaderTitle { get; set; }
    public string? Government { get; set; }
    public bool IsNamed() => string.IsNullOrEmpty(Name) is false;
    public string? FormalLeader
    {
        get
        {
            if (Name is null) return null;
            return $"{LeaderTitle ?? "Leader"} of {Government ?? "Nation"} of {Name ?? "tbd"}";
        }
    }
    public string? FormalNation
    {
        get
        {
            if (Name is null) return null;
            return $"{Government ?? "Nation"} of {Name ?? "tbd"}";
        }
    }
    public NationNaming DeepCopy()
    {
        return new NationNaming()
        {
            NationCode = NationCode,
            Name = Name,
            Possessive = Possessive,
            LeaderTitle = LeaderTitle,
            Government = Government
        };
    }
}

public enum NationRequestType { NONE = 0, Get = 10, Update = 20, AcceptReject = 30, Resign = 99 }