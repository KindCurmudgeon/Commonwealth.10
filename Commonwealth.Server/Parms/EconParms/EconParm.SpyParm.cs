using Commonwealth.Shared.Common;

namespace Commonwealth.Server.Parameters;

// public partial class SpyParms
// { 
//     [SetsRequiredMembers]
//     public SpyParms(SpyParmsRaw raw)
//     {
//         Training = raw.Training;
//         Operation = raw.Operation;
//         Transfer = raw.Transfer;
//     }
//     public static SpyParms Default = new()
//     {
//         Training = null,
//         Operation = null,
//         Transfer = null
//     };
// }
public class SpyParmsRaw
{
    public List<Asset>? Training { get; set; }
    public List<Asset>? Operation { get; set; }
    public List<Asset>? Transfer { get; set; }
}