namespace Commonwealth.Server.Utilities;

public static class BlobNaming
{
    public const string Json = ".json";
    public const string GameSuffix = "-game";
    public const string GameParmsSuffix = "-parms";
    public const string NationSuffix = "-nation-";

 //   public const string StatusPrefix = "-orders-";
 //   public const string SeasonSuffix = "-season";
    public const string HistorySuffix = "-history-";
}

public static class Folders
{
    public static string Parms { get; set; } = "Parms";
    public static string Games { get; set; } = "Games";
    public static string Users {get;set;} = "Users";
}
