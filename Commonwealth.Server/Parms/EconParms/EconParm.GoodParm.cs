namespace Commonwealth.Server.Parameters;

// public partial class GoodParm
// {
// //     [SetsRequiredMembers]
// //     public GoodParm(GoodParmFile goodParmRaw)
// //     {
// //         Name = goodParmRaw.Name ?? throw new AppException(ExceptionType.Blob, BlobFailType.FileContent, "GoodParmFile: Good Name");
// //  //       Type = Util.StringToEnum<GoodType>(goodParmRaw.Type);
// // //        Abbr = goodParmRaw.Abbr ?? goodParmRaw.Name;
// //     }
// }
public class GoodParmFile
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Abbr { get; set; }
    // public string? Units { get; set; }
    // public int? Initial { get; set; }
}