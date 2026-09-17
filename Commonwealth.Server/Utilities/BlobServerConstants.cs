namespace Commonwealth.Server.Utilities;

public static class BlobNaming
{
    public const string Json = ".json";
    public const string GameSetupSuffix = "-gamesetup";
    public const string GameStatusSuffix = "-gamestatus";
    public const string WorldSetupSuffix = "-worldsetup";
    public const string WorldStatusSuffix = "-worldstatus";
    // public const string GameSetupSuffix = "-setup";
    // public const string GameStatusSuffix = "-status";
    public const string GameParmsSuffix = "-parms";
    public const string NationSuffix = "-nation-";
    public const string HistorySuffix = "-history-";
}

public static class Folders
{
    public const string Parms = "Parms";
    public const string Games = "Games";
    public const string Players = "Players";
    public const string UserIdentities = "UsersIdentities";
    public static string AllGamesBlobPrefix() { return BlobService.CreateBlobPrefix(Folders.Games, null, null); }
    public static string AllPlayersPrefix() { return BlobService.CreateBlobPrefix(Folders.Players, null, null); }
}
