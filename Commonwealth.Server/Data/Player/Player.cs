using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Data;

public partial class Player
{
    public Guid Id { get; set; }
    public List<NationIdentity> NationIdentities { get; set; } = [];
    public List<string> Friends { get; set; } = [];
    [JsonConstructor] public Player() { }

}





