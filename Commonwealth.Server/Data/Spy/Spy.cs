using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace Commonwealth.Server.Data;

public partial class Spy
{
    public Guid Id { get; set; }
    public int CodeName { get; set; }
    public Report? Report { get; set; }
    public string? District { get; set; }
    public required SpyOrder Order { get; set; }
    [JsonConstructor] public Spy() { }
    
    [SetsRequiredMembers]
    public Spy(SpyMgr mgr)
    {
        Id = mgr.Id;
        CodeName = 0;
        Order = mgr.Order;
        District = null;
        Report = null;
    }


}