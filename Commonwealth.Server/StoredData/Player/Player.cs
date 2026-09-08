using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Data;

public partial class Player
{
    public int Version { get; set; } = 1;
    public required string UserName { get; set; }
    public List<NationIdentity> NationIdentities { get; set; } = [];
    public List<string> Friends { get; set; } = [];
    [JsonConstructor] public Player() { }

}
public static class RoleLevel
{
    public const int BASE = 0;
    public const int Admin = 50;
    public const int Developer = 100;
}





